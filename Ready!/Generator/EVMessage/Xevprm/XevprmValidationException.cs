using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;
using Ready.Model;

namespace EVMessage.Xevprm
{
    public enum SeverityType
    {
        NULL,
        Error,
        Warning
    }

    public enum ValidationExceptionDestination
    {
        NULL,
        Ready,
        Evprm,
        ReadyAndEvprm
    }

    public class XevprmValidationException
    {
        #region Declarations

        XevprmOperationType _operationType;
        SeverityType _severity;
        ValidationExceptionDestination _exceptionDestination;

        List<string> _relatedBusinessRuleIdList;

        #endregion

        #region Properties

        //Evprm
        public string EvprmMessage { get; set; }
        public string EvprmPropertyName { get; set; }
        public string EvprmPropertyValue { get; set; }
        public string EvprmLocation { get; set; }

        //Ready
        public string ReadyMessage { get; set; }
        public Type ReadyRootEntityType { get; set; }
        public int? ReadyRootEntityPk { get; set; }
        public Type ReadyEntityType { get; set; }
        public int? ReadyEntityPk { get; set; }
        public string ReadyEntityPropertyName { get; set; }
        public object ReadyEntityPropertyValue { get; set; }
        public string NavigateUrl { get; set; }
        
        public string XevprmValidationRuleId { get; set; }
        public string XevprmBusinessRule { get; set; }

        public List<string> RelatedBusinessRuleList
        {
            get { return _relatedBusinessRuleIdList; }
            set { _relatedBusinessRuleIdList = value; }
        }

        public SeverityType Severity
        {
            get { return _severity; }
            set { _severity = value; }
        }

        public XevprmOperationType OperationType
        {
            get { return _operationType; }
            set { _operationType = value; }
        }

        public ValidationExceptionDestination ExceptionDestination
        {
            get { return _exceptionDestination; }
            set { _exceptionDestination = value; }
        }

        #endregion

        #region Constructors

        public XevprmValidationException(IXevprmValidationRule xevprmValidationRule, XevprmOperationType operationType = XevprmOperationType.Insert, SeverityType severity = SeverityType.Error)
        {
            XevprmValidationRuleId = xevprmValidationRule.Id;
            ReadyMessage = xevprmValidationRule.ReadyMessage;
            EvprmMessage = xevprmValidationRule.EvprmMessage;
            XevprmBusinessRule = xevprmValidationRule.BusinessRule;

            _operationType = operationType;
            _severity = severity;

            _relatedBusinessRuleIdList = new List<string>();

            _exceptionDestination = ValidationExceptionDestination.NULL;
        }

        public XevprmValidationException(Type rootReadyType, int? rootReadyId, XevprmOperationType operationType = XevprmOperationType.Insert, SeverityType severity = SeverityType.Error)
        {
            ReadyRootEntityType = rootReadyType;
            ReadyRootEntityPk = rootReadyId;
            ReadyEntityType = rootReadyType;
            ReadyEntityPk = rootReadyId;

            _operationType = operationType;
            _severity = severity;

            _relatedBusinessRuleIdList = new List<string>();

            _exceptionDestination = ValidationExceptionDestination.NULL;
        }

        #endregion

        #region Methods

        public void AddEvprmDescription(string evprmLocation, string evprmPropertyName, string evprmPropertyValue)
        {
            EvprmLocation = evprmLocation;
            EvprmPropertyName = evprmPropertyName;
            EvprmPropertyValue = evprmPropertyValue;

            if (_exceptionDestination == ValidationExceptionDestination.NULL || _exceptionDestination == ValidationExceptionDestination.Evprm)
            {
                _exceptionDestination = ValidationExceptionDestination.Evprm;
            }
            else
            {
                _exceptionDestination = ValidationExceptionDestination.ReadyAndEvprm;
            }
        }

        public void AddReadyDescription(string navigateUrl, Type entityType, int? entityId, string propertyName, object propertyValue)
        {
            NavigateUrl = navigateUrl;
            ReadyEntityPk = entityId;
            ReadyEntityType = entityType;
            ReadyEntityPropertyName = propertyName;
            ReadyEntityPropertyValue = propertyValue;

            ReadyRootEntityType = ReadyEntityType;
            ReadyRootEntityPk = ReadyEntityPk;

            if (_exceptionDestination == ValidationExceptionDestination.NULL || _exceptionDestination == ValidationExceptionDestination.Ready)
            {
                _exceptionDestination = ValidationExceptionDestination.Ready;
            }
            else
            {
                _exceptionDestination = ValidationExceptionDestination.ReadyAndEvprm;
            }
        }
        public void AddReadyDescription<T2>(string navigateUrl, Expression<Func<int?>> entityPk, Expression<Func<T2>> entityProperty)
        {
            NavigateUrl = navigateUrl;

            ReadyEntityType = ((MemberExpression)entityPk.Body).Expression.Type;
            ReadyEntityPk = entityPk.Compile()();

            ReadyRootEntityType = ReadyEntityType;
            ReadyRootEntityPk = ReadyEntityPk;

            ReadyEntityPropertyName = ((MemberExpression)entityProperty.Body).Member.Name;
            ReadyEntityPropertyValue = entityProperty.Compile()();

            if (_exceptionDestination == ValidationExceptionDestination.NULL || _exceptionDestination == ValidationExceptionDestination.Ready)
            {
                _exceptionDestination = ValidationExceptionDestination.Ready;
            }
            else
            {
                _exceptionDestination = ValidationExceptionDestination.ReadyAndEvprm;
            }
        }

        public void AddReadyDescription<T1>(string navigateUrl, Expression<Func<int?>> rootEntityPk, Expression<Func<int?>> entityPk, Expression<Func<T1>> entityProperty)
        {
            NavigateUrl = navigateUrl;

            ReadyRootEntityType = ((MemberExpression)rootEntityPk.Body).Expression.Type;
            ReadyRootEntityPk = rootEntityPk.Compile()();

            ReadyEntityType = ((MemberExpression)entityPk.Body).Expression.Type;
            ReadyEntityPk = entityPk.Compile()();

            ReadyEntityPropertyName = ((MemberExpression)entityProperty.Body).Member.Name;
            ReadyEntityPropertyValue = entityProperty.Compile()();

            if (_exceptionDestination == ValidationExceptionDestination.NULL || _exceptionDestination == ValidationExceptionDestination.Ready)
            {
                _exceptionDestination = ValidationExceptionDestination.Ready;
            }
            else
            {
                _exceptionDestination = ValidationExceptionDestination.ReadyAndEvprm;
            }
        }

        #endregion
    }
}

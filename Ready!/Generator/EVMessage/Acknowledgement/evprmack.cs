using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Xml.Schema.Linq;

namespace EVMessage.Acknowledgement
{
    /// <summary>
    /// <para>
    /// Regular expression: (ichicsrmessageheader, acknowledgment)
    /// </para>
    /// </summary>
    public partial class evprmack : XTypedElement, IXMetaData
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static Dictionary<XName, System.Type> localElementDictionary = new Dictionary<XName, System.Type>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ContentModelEntity contentModel;

        public static explicit operator evprmack(XElement xe) { return XTypedServices.ToXTypedElement<evprmack>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        static evprmack()
        {
            BuildElementDictionary();
            contentModel = new SequenceContentModelEntity(new NamedContentModelEntity(XName.Get("ichicsrmessageheader", "")), new NamedContentModelEntity(XName.Get("acknowledgment", "")));
        }

        /// <summary>
        /// <para>
        /// Regular expression: (ichicsrmessageheader, acknowledgment)
        /// </para>
        /// </summary>
        public evprmack()
        {
        }

        /// <summary>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (ichicsrmessageheader, acknowledgment)
        /// </para>
        /// </summary>
        public ichicsrmessageheader ichicsrmessageheader
        {
            get
            {
                XElement x = this.GetElement(XName.Get("ichicsrmessageheader", ""));
                return ((ichicsrmessageheader)(x));
            }
            set
            {
                this.SetElement(XName.Get("ichicsrmessageheader", ""), value);
            }
        }

        /// <summary>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (ichicsrmessageheader, acknowledgment)
        /// </para>
        /// </summary>
        public acknowledgment acknowledgment
        {
            get
            {
                XElement x = this.GetElement(XName.Get("acknowledgment", ""));
                return ((acknowledgment)(x));
            }
            set
            {
                this.SetElement(XName.Get("acknowledgment", ""), value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Dictionary<XName, System.Type> IXMetaData.LocalElementsDictionary
        {
            get
            {
                return localElementDictionary;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("evprmack", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static evprmack Load(string xmlFile)
        {
            return XTypedServices.Load<evprmack>(xmlFile);
        }

        public static evprmack Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<evprmack>(xmlFile);
        }

        public static evprmack Parse(string xml)
        {
            return XTypedServices.Parse<evprmack>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<evprmack>(this);
        }

        private static void BuildElementDictionary()
        {
            localElementDictionary.Add(XName.Get("ichicsrmessageheader", ""), typeof(ichicsrmessageheader));
            localElementDictionary.Add(XName.Get("acknowledgment", ""), typeof(acknowledgment));
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return contentModel;
        }
    }

    /// <summary>
    /// <para>
    /// Regular expression: (messagetype, messageformatversion, messageformatrelease, messagenumb, messagesenderidentifier, messagereceiveridentifier, messagedateformat, messagedate)
    /// </para>
    /// </summary>
    public partial class ichicsrmessageheader : XTypedElement, IXMetaData
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static Dictionary<XName, System.Type> localElementDictionary = new Dictionary<XName, System.Type>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ContentModelEntity contentModel;

        public static explicit operator ichicsrmessageheader(XElement xe) { return XTypedServices.ToXTypedElement<ichicsrmessageheader>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        static ichicsrmessageheader()
        {
            BuildElementDictionary();
            contentModel = new SequenceContentModelEntity(new NamedContentModelEntity(XName.Get("messagetype", "")), new NamedContentModelEntity(XName.Get("messageformatversion", "")), new NamedContentModelEntity(XName.Get("messageformatrelease", "")), new NamedContentModelEntity(XName.Get("messagenumb", "")), new NamedContentModelEntity(XName.Get("messagesenderidentifier", "")), new NamedContentModelEntity(XName.Get("messagereceiveridentifier", "")), new NamedContentModelEntity(XName.Get("messagedateformat", "")), new NamedContentModelEntity(XName.Get("messagedate", "")));
        }

        /// <summary>
        /// <para>
        /// Regular expression: (messagetype, messageformatversion, messageformatrelease, messagenumb, messagesenderidentifier, messagereceiveridentifier, messagedateformat, messagedate)
        /// </para>
        /// </summary>
        public ichicsrmessageheader()
        {
        }

        /// <summary>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (messagetype, messageformatversion, messageformatrelease, messagenumb, messagesenderidentifier, messagereceiveridentifier, messagedateformat, messagedate)
        /// </para>
        /// </summary>
        public messagetype messagetype
        {
            get
            {
                XElement x = this.GetElement(XName.Get("messagetype", ""));
                return ((messagetype)(x));
            }
            set
            {
                this.SetElement(XName.Get("messagetype", ""), value);
            }
        }

        /// <summary>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (messagetype, messageformatversion, messageformatrelease, messagenumb, messagesenderidentifier, messagereceiveridentifier, messagedateformat, messagedate)
        /// </para>
        /// </summary>
        public messageformatversion messageformatversion
        {
            get
            {
                XElement x = this.GetElement(XName.Get("messageformatversion", ""));
                return ((messageformatversion)(x));
            }
            set
            {
                this.SetElement(XName.Get("messageformatversion", ""), value);
            }
        }

        /// <summary>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (messagetype, messageformatversion, messageformatrelease, messagenumb, messagesenderidentifier, messagereceiveridentifier, messagedateformat, messagedate)
        /// </para>
        /// </summary>
        public messageformatrelease messageformatrelease
        {
            get
            {
                XElement x = this.GetElement(XName.Get("messageformatrelease", ""));
                return ((messageformatrelease)(x));
            }
            set
            {
                this.SetElement(XName.Get("messageformatrelease", ""), value);
            }
        }

        /// <summary>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (messagetype, messageformatversion, messageformatrelease, messagenumb, messagesenderidentifier, messagereceiveridentifier, messagedateformat, messagedate)
        /// </para>
        /// </summary>
        public messagenumb messagenumb
        {
            get
            {
                XElement x = this.GetElement(XName.Get("messagenumb", ""));
                return ((messagenumb)(x));
            }
            set
            {
                this.SetElement(XName.Get("messagenumb", ""), value);
            }
        }

        /// <summary>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (messagetype, messageformatversion, messageformatrelease, messagenumb, messagesenderidentifier, messagereceiveridentifier, messagedateformat, messagedate)
        /// </para>
        /// </summary>
        public messagesenderidentifier messagesenderidentifier
        {
            get
            {
                XElement x = this.GetElement(XName.Get("messagesenderidentifier", ""));
                return ((messagesenderidentifier)(x));
            }
            set
            {
                this.SetElement(XName.Get("messagesenderidentifier", ""), value);
            }
        }

        /// <summary>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (messagetype, messageformatversion, messageformatrelease, messagenumb, messagesenderidentifier, messagereceiveridentifier, messagedateformat, messagedate)
        /// </para>
        /// </summary>
        public messagereceiveridentifier messagereceiveridentifier
        {
            get
            {
                XElement x = this.GetElement(XName.Get("messagereceiveridentifier", ""));
                return ((messagereceiveridentifier)(x));
            }
            set
            {
                this.SetElement(XName.Get("messagereceiveridentifier", ""), value);
            }
        }

        /// <summary>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (messagetype, messageformatversion, messageformatrelease, messagenumb, messagesenderidentifier, messagereceiveridentifier, messagedateformat, messagedate)
        /// </para>
        /// </summary>
        public messagedateformat messagedateformat
        {
            get
            {
                XElement x = this.GetElement(XName.Get("messagedateformat", ""));
                return ((messagedateformat)(x));
            }
            set
            {
                this.SetElement(XName.Get("messagedateformat", ""), value);
            }
        }

        /// <summary>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (messagetype, messageformatversion, messageformatrelease, messagenumb, messagesenderidentifier, messagereceiveridentifier, messagedateformat, messagedate)
        /// </para>
        /// </summary>
        public messagedate messagedate
        {
            get
            {
                XElement x = this.GetElement(XName.Get("messagedate", ""));
                return ((messagedate)(x));
            }
            set
            {
                this.SetElement(XName.Get("messagedate", ""), value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Dictionary<XName, System.Type> IXMetaData.LocalElementsDictionary
        {
            get
            {
                return localElementDictionary;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("ichicsrmessageheader", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static ichicsrmessageheader Load(string xmlFile)
        {
            return XTypedServices.Load<ichicsrmessageheader>(xmlFile);
        }

        public static ichicsrmessageheader Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<ichicsrmessageheader>(xmlFile);
        }

        public static ichicsrmessageheader Parse(string xml)
        {
            return XTypedServices.Parse<ichicsrmessageheader>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<ichicsrmessageheader>(this);
        }

        private static void BuildElementDictionary()
        {
            localElementDictionary.Add(XName.Get("messagetype", ""), typeof(messagetype));
            localElementDictionary.Add(XName.Get("messageformatversion", ""), typeof(messageformatversion));
            localElementDictionary.Add(XName.Get("messageformatrelease", ""), typeof(messageformatrelease));
            localElementDictionary.Add(XName.Get("messagenumb", ""), typeof(messagenumb));
            localElementDictionary.Add(XName.Get("messagesenderidentifier", ""), typeof(messagesenderidentifier));
            localElementDictionary.Add(XName.Get("messagereceiveridentifier", ""), typeof(messagereceiveridentifier));
            localElementDictionary.Add(XName.Get("messagedateformat", ""), typeof(messagedateformat));
            localElementDictionary.Add(XName.Get("messagedate", ""), typeof(messagedate));
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return contentModel;
        }
    }

    /// <summary>
    /// <para>
    /// Regular expression: (evmessagenumb, originalmessagenumb, originalmessagesenderidentifier, originalmessagereceiveridentifier, originalmessagedateformat, originalmessagedate, transmissionacknowledgmentcode, parsingerrormessage?)
    /// </para>
    /// </summary>
    public partial class messageacknowledgment : XTypedElement, IXMetaData
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static Dictionary<XName, System.Type> localElementDictionary = new Dictionary<XName, System.Type>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ContentModelEntity contentModel;

        public static explicit operator messageacknowledgment(XElement xe) { return XTypedServices.ToXTypedElement<messageacknowledgment>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        static messageacknowledgment()
        {
            BuildElementDictionary();
            contentModel = new SequenceContentModelEntity(new NamedContentModelEntity(XName.Get("evmessagenumb", "")), new NamedContentModelEntity(XName.Get("originalmessagenumb", "")), new NamedContentModelEntity(XName.Get("originalmessagesenderidentifier", "")), new NamedContentModelEntity(XName.Get("originalmessagereceiveridentifier", "")), new NamedContentModelEntity(XName.Get("originalmessagedateformat", "")), new NamedContentModelEntity(XName.Get("originalmessagedate", "")), new NamedContentModelEntity(XName.Get("transmissionacknowledgmentcode", "")), new NamedContentModelEntity(XName.Get("parsingerrormessage", "")));
        }

        /// <summary>
        /// <para>
        /// Regular expression: (evmessagenumb, originalmessagenumb, originalmessagesenderidentifier, originalmessagereceiveridentifier, originalmessagedateformat, originalmessagedate, transmissionacknowledgmentcode, parsingerrormessage?)
        /// </para>
        /// </summary>
        public messageacknowledgment()
        {
        }

        /// <summary>
        /// <para>
        /// The code associated to message we are sending the acknowledgement by EV System
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (evmessagenumb, originalmessagenumb, originalmessagesenderidentifier, originalmessagereceiveridentifier, originalmessagedateformat, originalmessagedate, transmissionacknowledgmentcode, parsingerrormessage?)
        /// </para>
        /// </summary>
        public string evmessagenumb
        {
            get
            {
                XElement x = this.GetElement(XName.Get("evmessagenumb", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("evmessagenumb", ""), value, "evmessagenumb", evmessagenumbLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// The Message Number of the message, Locally Assigned, we are sending the acknowledgement
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (evmessagenumb, originalmessagenumb, originalmessagesenderidentifier, originalmessagereceiveridentifier, originalmessagedateformat, originalmessagedate, transmissionacknowledgmentcode, parsingerrormessage?)
        /// </para>
        /// </summary>
        public string originalmessagenumb
        {
            get
            {
                XElement x = this.GetElement(XName.Get("originalmessagenumb", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("originalmessagenumb", ""), value, "originalmessagenumb", originalmessagenumbLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// The Message Sender Identifier of the message we are sending the acknowledgement
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (evmessagenumb, originalmessagenumb, originalmessagesenderidentifier, originalmessagereceiveridentifier, originalmessagedateformat, originalmessagedate, transmissionacknowledgmentcode, parsingerrormessage?)
        /// </para>
        /// </summary>
        public string originalmessagesenderidentifier
        {
            get
            {
                XElement x = this.GetElement(XName.Get("originalmessagesenderidentifier", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("originalmessagesenderidentifier", ""), value, "originalmessagesenderidentifier", originalmessagesenderidentifierLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// The Message Receiver Identifier of the message we are sending the acknowledgement
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (evmessagenumb, originalmessagenumb, originalmessagesenderidentifier, originalmessagereceiveridentifier, originalmessagedateformat, originalmessagedate, transmissionacknowledgmentcode, parsingerrormessage?)
        /// </para>
        /// </summary>
        public string originalmessagereceiveridentifier
        {
            get
            {
                XElement x = this.GetElement(XName.Get("originalmessagereceiveridentifier", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("originalmessagereceiveridentifier", ""), value, "originalmessagereceiveridentifier", originalmessagereceiveridentifierLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// The format of the Original Message Date.
        /// </para>
        /// <para>
        /// The unique value admitted is "204" corresponding at "CCYYMMDDHHMMSS"
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (evmessagenumb, originalmessagenumb, originalmessagesenderidentifier, originalmessagereceiveridentifier, originalmessagedateformat, originalmessagedate, transmissionacknowledgmentcode, parsingerrormessage?)
        /// </para>
        /// </summary>
        public string originalmessagedateformat
        {
            get
            {
                XElement x = this.GetElement(XName.Get("originalmessagedateformat", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("originalmessagedateformat", ""), value, "originalmessagedateformat", originalmessagedateformatLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// The Original Message Date.
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (evmessagenumb, originalmessagenumb, originalmessagesenderidentifier, originalmessagereceiveridentifier, originalmessagedateformat, originalmessagedate, transmissionacknowledgmentcode, parsingerrormessage?)
        /// </para>
        /// </summary>
        public string originalmessagedate
        {
            get
            {
                XElement x = this.GetElement(XName.Get("originalmessagedate", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("originalmessagedate", ""), value, "originalmessagedate", originalmessagedateLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// 01= All Reports loaded into database 02 = EVPR Error, not all reports loaded into the database, check section B 03= SGML parsing error, no data extracted 
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (evmessagenumb, originalmessagenumb, originalmessagesenderidentifier, originalmessagereceiveridentifier, originalmessagedateformat, originalmessagedate, transmissionacknowledgmentcode, parsingerrormessage?)
        /// </para>
        /// </summary>
        public string transmissionacknowledgmentcode
        {
            get
            {
                XElement x = this.GetElement(XName.Get("transmissionacknowledgmentcode", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("transmissionacknowledgmentcode", ""), value, "transmissionacknowledgmentcode", transmissionacknowledgmentcodeLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// This field contains the description when happened an error. It's mandatory when transmission acknowledgment code is 03.
        /// </para>
        /// <para>
        /// Occurrence: optional
        /// </para>
        /// <para>
        /// Regular expression: (evmessagenumb, originalmessagenumb, originalmessagesenderidentifier, originalmessagereceiveridentifier, originalmessagedateformat, originalmessagedate, transmissionacknowledgmentcode, parsingerrormessage?)
        /// </para>
        /// </summary>
        public string parsingerrormessage
        {
            get
            {
                XElement x = this.GetElement(XName.Get("parsingerrormessage", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("parsingerrormessage", ""), value, "parsingerrormessage", parsingerrormessageLocalType.TypeDefinition);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Dictionary<XName, System.Type> IXMetaData.LocalElementsDictionary
        {
            get
            {
                return localElementDictionary;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("messageacknowledgment", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static messageacknowledgment Load(string xmlFile)
        {
            return XTypedServices.Load<messageacknowledgment>(xmlFile);
        }

        public static messageacknowledgment Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<messageacknowledgment>(xmlFile);
        }

        public static messageacknowledgment Parse(string xml)
        {
            return XTypedServices.Parse<messageacknowledgment>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<messageacknowledgment>(this);
        }

        private static void BuildElementDictionary()
        {
            localElementDictionary.Add(XName.Get("evmessagenumb", ""), typeof(string));
            localElementDictionary.Add(XName.Get("originalmessagenumb", ""), typeof(string));
            localElementDictionary.Add(XName.Get("originalmessagesenderidentifier", ""), typeof(string));
            localElementDictionary.Add(XName.Get("originalmessagereceiveridentifier", ""), typeof(string));
            localElementDictionary.Add(XName.Get("originalmessagedateformat", ""), typeof(string));
            localElementDictionary.Add(XName.Get("originalmessagedate", ""), typeof(string));
            localElementDictionary.Add(XName.Get("transmissionacknowledgmentcode", ""), typeof(string));
            localElementDictionary.Add(XName.Get("parsingerrormessage", ""), typeof(string));
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return contentModel;
        }

        private class evmessagenumbLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 100, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private evmessagenumbLocalType()
            {
            }
        }

        private class originalmessagenumbLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 100, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private originalmessagenumbLocalType()
            {
            }
        }

        private class originalmessagesenderidentifierLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 60, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private originalmessagesenderidentifierLocalType()
            {
            }
        }

        private class originalmessagereceiveridentifierLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 60, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private originalmessagereceiveridentifierLocalType()
            {
            }
        }

        private class originalmessagedateformatLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(17)), new object[] {
                        "204"}, 0, 3, null, null, 0, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private originalmessagedateformatLocalType()
            {
            }
        }

        private class originalmessagedateLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(1)), null, 0, 14, null, null, 0, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private originalmessagedateLocalType()
            {
            }
        }

        private class transmissionacknowledgmentcodeLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(17)), new object[] {
                        "01",
                        "02",
                        "03"}, 0, 2, null, null, 0, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private transmissionacknowledgmentcodeLocalType()
            {
            }
        }

        private class parsingerrormessageLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), null);

            private parsingerrormessageLocalType()
            {
            }
        }
    }

    /// <summary>
    /// <para>
    /// Regular expression: (reportname, localnumber?, ev_code?, operationtype, operationresult, operationresultdesc, reportcomments?)
    /// </para>
    /// </summary>
    public partial class reportacknowledgment : XTypedElement, IXMetaData
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static Dictionary<XName, System.Type> localElementDictionary = new Dictionary<XName, System.Type>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ContentModelEntity contentModel;

        public static explicit operator reportacknowledgment(XElement xe) { return XTypedServices.ToXTypedElement<reportacknowledgment>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        static reportacknowledgment()
        {
            BuildElementDictionary();
            contentModel = new SequenceContentModelEntity(new NamedContentModelEntity(XName.Get("reportname", "")), new NamedContentModelEntity(XName.Get("localnumber", "")), new NamedContentModelEntity(XName.Get("ev_code", "")), new NamedContentModelEntity(XName.Get("operationtype", "")), new NamedContentModelEntity(XName.Get("operationresult", "")), new NamedContentModelEntity(XName.Get("operationresultdesc", "")), new NamedContentModelEntity(XName.Get("reportcomments", "")));
        }

        /// <summary>
        /// <para>
        /// Regular expression: (reportname, localnumber?, ev_code?, operationtype, operationresult, operationresultdesc, reportcomments?)
        /// </para>
        /// </summary>
        public reportacknowledgment()
        {
        }

        /// <summary>
        /// <para>
        /// This field says the type section of message
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (reportname, localnumber?, ev_code?, operationtype, operationresult, operationresultdesc, reportcomments?)
        /// </para>
        /// </summary>
        public string reportname
        {
            get
            {
                XElement x = this.GetElement(XName.Get("reportname", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("reportname", ""), value, "reportname", reportnameLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// This field is the number assigned by the sender to identify report in the position of XML file. May be Null for no Inserts, and it's mandatory for Inserts.
        /// </para>
        /// <para>
        /// Occurrence: optional
        /// </para>
        /// <para>
        /// Regular expression: (reportname, localnumber?, ev_code?, operationtype, operationresult, operationresultdesc, reportcomments?)
        /// </para>
        /// </summary>
        public string localnumber
        {
            get
            {
                XElement x = this.GetElement(XName.Get("localnumber", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("localnumber", ""), value, "localnumber", localnumberLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// This field is the number assigned by the EVMPD in case of an INSERT or by the sender, in all other cases, to identify the report in the EVMPD. May be Null for the insucessfull Inserts (see operationresult)
        /// </para>
        /// <para>
        /// Occurrence: optional
        /// </para>
        /// <para>
        /// Regular expression: (reportname, localnumber?, ev_code?, operationtype, operationresult, operationresultdesc, reportcomments?)
        /// </para>
        /// </summary>
        public string ev_code
        {
            get
            {
                XElement x = this.GetElement(XName.Get("ev_code", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("ev_code", ""), value, "ev_code", ev_codeLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// This field says which operation type wanted to do in each section (1=Insert, 2=Update, 3=Variation, 4=Nullify, 5=Change Ownership, 6=Withdrawn).
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (reportname, localnumber?, ev_code?, operationtype, operationresult, operationresultdesc, reportcomments?)
        /// </para>
        /// </summary>
        public decimal operationtype
        {
            get
            {
                XElement x = this.GetElement(XName.Get("operationtype", ""));
                return XTypedServices.ParseValue<decimal>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.NonNegativeInteger).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("operationtype", ""), value, "operationtype", operationtypeLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// This field says the operation result of a particular section
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (reportname, localnumber?, ev_code?, operationtype, operationresult, operationresultdesc, reportcomments?)
        /// </para>
        /// </summary>
        public decimal operationresult
        {
            get
            {
                XElement x = this.GetElement(XName.Get("operationresult", ""));
                return XTypedServices.ParseValue<decimal>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.NonNegativeInteger).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("operationresult", ""), value, "operationresult", operationresultLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// This field says the description operation result of a particular section
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (reportname, localnumber?, ev_code?, operationtype, operationresult, operationresultdesc, reportcomments?)
        /// </para>
        /// </summary>
        public string operationresultdesc
        {
            get
            {
                XElement x = this.GetElement(XName.Get("operationresultdesc", ""));
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetElementWithValidation(XName.Get("operationresultdesc", ""), value, "operationresultdesc", operationresultdescLocalType.TypeDefinition);
            }
        }

        /// <summary>
        /// <para>
        /// The section reportcomments contains the list of errors and warnings detected by the message parsing process 
        /// </para>
        /// <para>
        /// Occurrence: optional
        /// </para>
        /// <para>
        /// Regular expression: (reportname, localnumber?, ev_code?, operationtype, operationresult, operationresultdesc, reportcomments?)
        /// </para>
        /// </summary>
        public reportcommentsLocalType reportcomments
        {
            get
            {
                XElement x = this.GetElement(XName.Get("reportcomments", ""));
                return ((reportcommentsLocalType)(x));
            }
            set
            {
                this.SetElement(XName.Get("reportcomments", ""), value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Dictionary<XName, System.Type> IXMetaData.LocalElementsDictionary
        {
            get
            {
                return localElementDictionary;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("reportacknowledgment", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static reportacknowledgment Load(string xmlFile)
        {
            return XTypedServices.Load<reportacknowledgment>(xmlFile);
        }

        public static reportacknowledgment Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<reportacknowledgment>(xmlFile);
        }

        public static reportacknowledgment Parse(string xml)
        {
            return XTypedServices.Parse<reportacknowledgment>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<reportacknowledgment>(this);
        }

        private static void BuildElementDictionary()
        {
            localElementDictionary.Add(XName.Get("reportname", ""), typeof(string));
            localElementDictionary.Add(XName.Get("localnumber", ""), typeof(string));
            localElementDictionary.Add(XName.Get("ev_code", ""), typeof(string));
            localElementDictionary.Add(XName.Get("operationtype", ""), typeof(decimal));
            localElementDictionary.Add(XName.Get("operationresult", ""), typeof(decimal));
            localElementDictionary.Add(XName.Get("operationresultdesc", ""), typeof(string));
            localElementDictionary.Add(XName.Get("reportcomments", ""), typeof(reportcommentsLocalType));
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return contentModel;
        }

        private class reportnameLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(20)), new object[] {
                        "ATTACHMENT",
                        "MFL",
                        "ORGANISATION",
                        "SOURCE",
                        "DEVELOPMENTSUBSTANCE",
                        "APPROVEDSUBSTANCE",
                        "ATCCODE",
                        "PHARMACEUTICALFORM",
                        "ADMINISTRATIONROUTE",
                        "DEVELOPMENTPRODUCT",
                        "AUTHORISEDPRODUCT"}, 0, 0, null, null, 60, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private reportnameLocalType()
            {
            }
        }

        private class localnumberLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 60, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private localnumberLocalType()
            {
            }
        }

        private class ev_codeLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 60, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private ev_codeLocalType()
            {
            }
        }

        private class operationtypeLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.NonNegativeInteger), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(64)), null, 0, 0, null, 8m, 0, null, null, 0, null, 0, XmlSchemaWhiteSpace.Collapse));

            private operationtypeLocalType()
            {
            }
        }

        private class operationresultLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.NonNegativeInteger), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(128)), null, 0, 0, 101m, null, 0, null, null, 0, null, 0, XmlSchemaWhiteSpace.Collapse));

            private operationresultLocalType()
            {
            }
        }

        private class operationresultdescLocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), null);

            private operationresultdescLocalType()
            {
            }
        }

        /// <summary>
        /// <para>
        /// Regular expression: (reportcomment+)
        /// </para>
        /// </summary>
        public partial class reportcommentsLocalType : XTypedElement, IXMetaData
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private XTypedList<reportcommentLocalType> reportcommentField;

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            static Dictionary<XName, System.Type> localElementDictionary = new Dictionary<XName, System.Type>();

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private static ContentModelEntity contentModel;

            public static explicit operator reportcommentsLocalType(XElement xe) { return XTypedServices.ToXTypedElement<reportcommentsLocalType>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

            static reportcommentsLocalType()
            {
                BuildElementDictionary();
                contentModel = new SequenceContentModelEntity(new NamedContentModelEntity(XName.Get("reportcomment", "")));
            }

            /// <summary>
            /// <para>
            /// Regular expression: (reportcomment+)
            /// </para>
            /// </summary>
            public reportcommentsLocalType()
            {
            }

            /// <summary>
            /// <para>
            /// The message parsing process will add a specific report comment element for each parsing warning or error detected during the validation process
            /// </para>
            /// <para>
            /// Occurrence: required, repeating
            /// </para>
            /// <para>
            /// Regular expression: (reportcomment+)
            /// </para>
            /// </summary>
            public IList<reportacknowledgment.reportcommentsLocalType.reportcommentLocalType> reportcomment
            {
                get
                {
                    if ((this.reportcommentField == null))
                    {
                        this.reportcommentField = new XTypedList<reportcommentLocalType>(this, LinqToXsdTypeManager.Instance, XName.Get("reportcomment", ""));
                    }
                    return this.reportcommentField;
                }
                set
                {
                    if ((value == null))
                    {
                        this.reportcommentField = null;
                    }
                    else
                    {
                        if ((this.reportcommentField == null))
                        {
                            this.reportcommentField = XTypedList<reportcommentLocalType>.Initialize(this, LinqToXsdTypeManager.Instance, value, XName.Get("reportcomment", ""));
                        }
                        else
                        {
                            XTypedServices.SetList<reportcommentLocalType>(this.reportcommentField, value);
                        }
                    }
                }
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            Dictionary<XName, System.Type> IXMetaData.LocalElementsDictionary
            {
                get
                {
                    return localElementDictionary;
                }
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            XName IXMetaData.SchemaName
            {
                get
                {
                    return XName.Get("reportcomments", "");
                }
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            SchemaOrigin IXMetaData.TypeOrigin
            {
                get
                {
                    return SchemaOrigin.Fragment;
                }
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            ILinqToXsdTypeManager IXMetaData.TypeManager
            {
                get
                {
                    return LinqToXsdTypeManager.Instance;
                }
            }

            public override XTypedElement Clone()
            {
                return XTypedServices.CloneXTypedElement<reportcommentsLocalType>(this);
            }

            private static void BuildElementDictionary()
            {
                localElementDictionary.Add(XName.Get("reportcomment", ""), typeof(reportcommentLocalType));
            }

            ContentModelEntity IXMetaData.GetContentModel()
            {
                return contentModel;
            }

            /// <summary>
            /// <para>
            /// Regular expression: (severity, sectionname?, fieldname?, fieldvalue?, commentcode?, commenttext?)
            /// </para>
            /// </summary>
            public partial class reportcommentLocalType : XTypedElement, IXMetaData
            {

                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                static Dictionary<XName, System.Type> localElementDictionary = new Dictionary<XName, System.Type>();

                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private static ContentModelEntity contentModel;

                public static explicit operator reportcommentLocalType(XElement xe) { return XTypedServices.ToXTypedElement<reportcommentLocalType>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

                static reportcommentLocalType()
                {
                    BuildElementDictionary();
                    contentModel = new SequenceContentModelEntity(new NamedContentModelEntity(XName.Get("severity", "")), new NamedContentModelEntity(XName.Get("sectionname", "")), new NamedContentModelEntity(XName.Get("fieldname", "")), new NamedContentModelEntity(XName.Get("fieldvalue", "")), new NamedContentModelEntity(XName.Get("commentcode", "")), new NamedContentModelEntity(XName.Get("commenttext", "")));
                }

                /// <summary>
                /// <para>
                /// Regular expression: (severity, sectionname?, fieldname?, fieldvalue?, commentcode?, commenttext?)
                /// </para>
                /// </summary>
                public reportcommentLocalType()
                {
                }

                /// <summary>
                /// <para>
                /// The severity flag describes if the report comment is an error or a warning
                /// </para>
                /// <para>
                /// Occurrence: required
                /// </para>
                /// <para>
                /// Regular expression: (severity, sectionname?, fieldname?, fieldvalue?, commentcode?, commenttext?)
                /// </para>
                /// </summary>
                public decimal severity
                {
                    get
                    {
                        XElement x = this.GetElement(XName.Get("severity", ""));
                        return XTypedServices.ParseValue<decimal>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.NonNegativeInteger).Datatype);
                    }
                    set
                    {
                        this.SetElementWithValidation(XName.Get("severity", ""), value, "severity", severityLocalType.TypeDefinition);
                    }
                }

                /// <summary>
                /// <para>
                /// The XML message section which the comment is referred to
                /// </para>
                /// <para>
                /// Occurrence: optional
                /// </para>
                /// <para>
                /// Regular expression: (severity, sectionname?, fieldname?, fieldvalue?, commentcode?, commenttext?)
                /// </para>
                /// </summary>
                public string sectionname
                {
                    get
                    {
                        XElement x = this.GetElement(XName.Get("sectionname", ""));
                        return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
                    }
                    set
                    {
                        this.SetElementWithValidation(XName.Get("sectionname", ""), value, "sectionname", sectionnameLocalType.TypeDefinition);
                    }
                }

                /// <summary>
                /// <para>
                /// This field contains the field's name in which was produced the first error (if exists)
                /// </para>
                /// <para>
                /// Occurrence: optional
                /// </para>
                /// <para>
                /// Regular expression: (severity, sectionname?, fieldname?, fieldvalue?, commentcode?, commenttext?)
                /// </para>
                /// </summary>
                public string fieldname
                {
                    get
                    {
                        XElement x = this.GetElement(XName.Get("fieldname", ""));
                        return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
                    }
                    set
                    {
                        this.SetElementWithValidation(XName.Get("fieldname", ""), value, "fieldname", fieldnameLocalType.TypeDefinition);
                    }
                }

                /// <summary>
                /// <para>
                /// This field contains the value of field in which was produced the first error (if exists)
                /// </para>
                /// <para>
                /// Occurrence: optional
                /// </para>
                /// <para>
                /// Regular expression: (severity, sectionname?, fieldname?, fieldvalue?, commentcode?, commenttext?)
                /// </para>
                /// </summary>
                public string fieldvalue
                {
                    get
                    {
                        XElement x = this.GetElement(XName.Get("fieldvalue", ""));
                        return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
                    }
                    set
                    {
                        this.SetElementWithValidation(XName.Get("fieldvalue", ""), value, "fieldvalue", fieldvalueLocalType.TypeDefinition);
                    }
                }

                /// <summary>
                /// <para>
                /// This field contains a comment about the error type 
                /// </para>
                /// <para>
                /// Occurrence: optional
                /// </para>
                /// <para>
                /// Regular expression: (severity, sectionname?, fieldname?, fieldvalue?, commentcode?, commenttext?)
                /// </para>
                /// </summary>
                public System.Nullable<decimal> commentcode
                {
                    get
                    {
                        XElement x = this.GetElement(XName.Get("commentcode", ""));
                        if ((x == null))
                        {
                            return null;
                        }
                        return XTypedServices.ParseValue<decimal>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.NonNegativeInteger).Datatype);
                    }
                    set
                    {
                        this.SetElementWithValidation(XName.Get("commentcode", ""), value, "commentcode", commentcodeLocalType.TypeDefinition);
                    }
                }

                /// <summary>
                /// <para>
                /// This field contain a detailed explanation of the error
                /// </para>
                /// <para>
                /// Occurrence: optional
                /// </para>
                /// <para>
                /// Regular expression: (severity, sectionname?, fieldname?, fieldvalue?, commentcode?, commenttext?)
                /// </para>
                /// </summary>
                public string commenttext
                {
                    get
                    {
                        XElement x = this.GetElement(XName.Get("commenttext", ""));
                        return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
                    }
                    set
                    {
                        this.SetElementWithValidation(XName.Get("commenttext", ""), value, "commenttext", commenttextLocalType.TypeDefinition);
                    }
                }

                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                Dictionary<XName, System.Type> IXMetaData.LocalElementsDictionary
                {
                    get
                    {
                        return localElementDictionary;
                    }
                }

                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                XName IXMetaData.SchemaName
                {
                    get
                    {
                        return XName.Get("reportcomment", "");
                    }
                }

                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                SchemaOrigin IXMetaData.TypeOrigin
                {
                    get
                    {
                        return SchemaOrigin.Fragment;
                    }
                }

                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                ILinqToXsdTypeManager IXMetaData.TypeManager
                {
                    get
                    {
                        return LinqToXsdTypeManager.Instance;
                    }
                }

                public override XTypedElement Clone()
                {
                    return XTypedServices.CloneXTypedElement<reportcommentLocalType>(this);
                }

                private static void BuildElementDictionary()
                {
                    localElementDictionary.Add(XName.Get("severity", ""), typeof(decimal));
                    localElementDictionary.Add(XName.Get("sectionname", ""), typeof(string));
                    localElementDictionary.Add(XName.Get("fieldname", ""), typeof(string));
                    localElementDictionary.Add(XName.Get("fieldvalue", ""), typeof(string));
                    localElementDictionary.Add(XName.Get("commentcode", ""), typeof(decimal));
                    localElementDictionary.Add(XName.Get("commenttext", ""), typeof(string));
                }

                ContentModelEntity IXMetaData.GetContentModel()
                {
                    return contentModel;
                }

                private class severityLocalType
                {

                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.NonNegativeInteger), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(1040)), new object[] {
                                1m,
                                2m}, 0, 0, null, null, 0, null, null, 0, null, 1, XmlSchemaWhiteSpace.Collapse));

                    private severityLocalType()
                    {
                    }
                }

                private class sectionnameLocalType
                {

                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 60, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

                    private sectionnameLocalType()
                    {
                    }
                }

                private class fieldnameLocalType
                {

                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 60, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

                    private fieldnameLocalType()
                    {
                    }
                }

                private class fieldvalueLocalType
                {

                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), null);

                    private fieldvalueLocalType()
                    {
                    }
                }

                private class commentcodeLocalType
                {

                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.NonNegativeInteger), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(1024)), null, 0, 0, null, null, 0, null, null, 0, null, 3, XmlSchemaWhiteSpace.Collapse));

                    private commentcodeLocalType()
                    {
                    }
                }

                private class commenttextLocalType
                {

                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), null);

                    private commenttextLocalType()
                    {
                    }
                }
            }
        }
    }

    public partial class messagetype : XTypedElement, IXMetaData
    {

        public static explicit operator messagetype(XElement xe) { return XTypedServices.ToXTypedElement<messagetype>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        public messagetype()
        {
        }

        public string TypedValue
        {
            get
            {
                XElement x = this.Untyped;
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetValueWithValidation(value, "TypedValue", messagetype1LocalType.TypeDefinition);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("messagetype", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static messagetype Load(string xmlFile)
        {
            return XTypedServices.Load<messagetype>(xmlFile);
        }

        public static messagetype Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<messagetype>(xmlFile);
        }

        public static messagetype Parse(string xml)
        {
            return XTypedServices.Parse<messagetype>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<messagetype>(this);
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return ContentModelEntity.Default;
        }

        private class messagetype1LocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(20)), new object[] {
                        "EVPRACK",
                        "evprack"}, 0, 0, null, null, 16, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private messagetype1LocalType()
            {
            }
        }
    }

    public partial class messageformatversion : XTypedElement, IXMetaData
    {

        public static explicit operator messageformatversion(XElement xe) { return XTypedServices.ToXTypedElement<messageformatversion>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        public messageformatversion()
        {
        }

        public string TypedValue
        {
            get
            {
                XElement x = this.Untyped;
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetValueWithValidation(value, "TypedValue", messageformatversion1LocalType.TypeDefinition);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("messageformatversion", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static messageformatversion Load(string xmlFile)
        {
            return XTypedServices.Load<messageformatversion>(xmlFile);
        }

        public static messageformatversion Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<messageformatversion>(xmlFile);
        }

        public static messageformatversion Parse(string xml)
        {
            return XTypedServices.Parse<messageformatversion>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<messageformatversion>(this);
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return ContentModelEntity.Default;
        }

        private class messageformatversion1LocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 3, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private messageformatversion1LocalType()
            {
            }
        }
    }

    public partial class messageformatrelease : XTypedElement, IXMetaData
    {

        public static explicit operator messageformatrelease(XElement xe) { return XTypedServices.ToXTypedElement<messageformatrelease>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        public messageformatrelease()
        {
        }

        public string TypedValue
        {
            get
            {
                XElement x = this.Untyped;
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetValueWithValidation(value, "TypedValue", messageformatrelease1LocalType.TypeDefinition);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("messageformatrelease", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static messageformatrelease Load(string xmlFile)
        {
            return XTypedServices.Load<messageformatrelease>(xmlFile);
        }

        public static messageformatrelease Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<messageformatrelease>(xmlFile);
        }

        public static messageformatrelease Parse(string xml)
        {
            return XTypedServices.Parse<messageformatrelease>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<messageformatrelease>(this);
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return ContentModelEntity.Default;
        }

        private class messageformatrelease1LocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 3, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private messageformatrelease1LocalType()
            {
            }
        }
    }

    public partial class messagenumb : XTypedElement, IXMetaData
    {

        public static explicit operator messagenumb(XElement xe) { return XTypedServices.ToXTypedElement<messagenumb>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        public messagenumb()
        {
        }

        public string TypedValue
        {
            get
            {
                XElement x = this.Untyped;
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetValueWithValidation(value, "TypedValue", messagenumb1LocalType.TypeDefinition);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("messagenumb", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static messagenumb Load(string xmlFile)
        {
            return XTypedServices.Load<messagenumb>(xmlFile);
        }

        public static messagenumb Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<messagenumb>(xmlFile);
        }

        public static messagenumb Parse(string xml)
        {
            return XTypedServices.Parse<messagenumb>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<messagenumb>(this);
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return ContentModelEntity.Default;
        }

        private class messagenumb1LocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 100, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private messagenumb1LocalType()
            {
            }
        }
    }

    public partial class messagesenderidentifier : XTypedElement, IXMetaData
    {

        public static explicit operator messagesenderidentifier(XElement xe) { return XTypedServices.ToXTypedElement<messagesenderidentifier>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        public messagesenderidentifier()
        {
        }

        public string TypedValue
        {
            get
            {
                XElement x = this.Untyped;
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetValueWithValidation(value, "TypedValue", messagesenderidentifier1LocalType.TypeDefinition);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("messagesenderidentifier", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static messagesenderidentifier Load(string xmlFile)
        {
            return XTypedServices.Load<messagesenderidentifier>(xmlFile);
        }

        public static messagesenderidentifier Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<messagesenderidentifier>(xmlFile);
        }

        public static messagesenderidentifier Parse(string xml)
        {
            return XTypedServices.Parse<messagesenderidentifier>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<messagesenderidentifier>(this);
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return ContentModelEntity.Default;
        }

        private class messagesenderidentifier1LocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 60, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private messagesenderidentifier1LocalType()
            {
            }
        }
    }

    public partial class messagereceiveridentifier : XTypedElement, IXMetaData
    {

        public static explicit operator messagereceiveridentifier(XElement xe) { return XTypedServices.ToXTypedElement<messagereceiveridentifier>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        public messagereceiveridentifier()
        {
        }

        public string TypedValue
        {
            get
            {
                XElement x = this.Untyped;
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetValueWithValidation(value, "TypedValue", messagereceiveridentifier1LocalType.TypeDefinition);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("messagereceiveridentifier", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static messagereceiveridentifier Load(string xmlFile)
        {
            return XTypedServices.Load<messagereceiveridentifier>(xmlFile);
        }

        public static messagereceiveridentifier Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<messagereceiveridentifier>(xmlFile);
        }

        public static messagereceiveridentifier Parse(string xml)
        {
            return XTypedServices.Parse<messagereceiveridentifier>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<messagereceiveridentifier>(this);
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return ContentModelEntity.Default;
        }

        private class messagereceiveridentifier1LocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(4)), null, 0, 0, null, null, 60, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private messagereceiveridentifier1LocalType()
            {
            }
        }
    }

    public partial class messagedateformat : XTypedElement, IXMetaData
    {

        public static explicit operator messagedateformat(XElement xe) { return XTypedServices.ToXTypedElement<messagedateformat>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        public messagedateformat()
        {
        }

        public string TypedValue
        {
            get
            {
                XElement x = this.Untyped;
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetValueWithValidation(value, "TypedValue", messagedateformat1LocalType.TypeDefinition);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("messagedateformat", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static messagedateformat Load(string xmlFile)
        {
            return XTypedServices.Load<messagedateformat>(xmlFile);
        }

        public static messagedateformat Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<messagedateformat>(xmlFile);
        }

        public static messagedateformat Parse(string xml)
        {
            return XTypedServices.Parse<messagedateformat>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<messagedateformat>(this);
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return ContentModelEntity.Default;
        }

        private class messagedateformat1LocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(17)), new object[] {
                        "204"}, 0, 3, null, null, 0, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private messagedateformat1LocalType()
            {
            }
        }
    }

    public partial class messagedate : XTypedElement, IXMetaData
    {

        public static explicit operator messagedate(XElement xe) { return XTypedServices.ToXTypedElement<messagedate>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        public messagedate()
        {
        }

        public string TypedValue
        {
            get
            {
                XElement x = this.Untyped;
                return XTypedServices.ParseValue<string>(x, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).Datatype);
            }
            set
            {
                this.SetValueWithValidation(value, "TypedValue", messagedate1LocalType.TypeDefinition);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("messagedate", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static messagedate Load(string xmlFile)
        {
            return XTypedServices.Load<messagedate>(xmlFile);
        }

        public static messagedate Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<messagedate>(xmlFile);
        }

        public static messagedate Parse(string xml)
        {
            return XTypedServices.Parse<messagedate>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<messagedate>(this);
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return ContentModelEntity.Default;
        }

        private class messagedate1LocalType
        {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public static Xml.Schema.Linq.SimpleTypeValidator TypeDefinition = new Xml.Schema.Linq.AtomicSimpleTypeValidator(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), new Xml.Schema.Linq.RestrictionFacets(((Xml.Schema.Linq.RestrictionFlags)(1)), null, 0, 14, null, null, 0, null, null, 0, null, 0, XmlSchemaWhiteSpace.Preserve));

            private messagedate1LocalType()
            {
            }
        }
    }

    /// <summary>
    /// <para>
    /// Regular expression: (messageacknowledgment, reportacknowledgment*)
    /// </para>
    /// </summary>
    public partial class acknowledgment : XTypedElement, IXMetaData
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private XTypedList<reportacknowledgment> reportacknowledgmentField;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static Dictionary<XName, System.Type> localElementDictionary = new Dictionary<XName, System.Type>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ContentModelEntity contentModel;

        public static explicit operator acknowledgment(XElement xe) { return XTypedServices.ToXTypedElement<acknowledgment>(xe, LinqToXsdTypeManager.Instance as ILinqToXsdTypeManager); }

        static acknowledgment()
        {
            BuildElementDictionary();
            contentModel = new SequenceContentModelEntity(new NamedContentModelEntity(XName.Get("messageacknowledgment", "")), new NamedContentModelEntity(XName.Get("reportacknowledgment", "")));
        }

        /// <summary>
        /// <para>
        /// Regular expression: (messageacknowledgment, reportacknowledgment*)
        /// </para>
        /// </summary>
        public acknowledgment()
        {
        }

        /// <summary>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Regular expression: (messageacknowledgment, reportacknowledgment*)
        /// </para>
        /// </summary>
        public messageacknowledgment messageacknowledgment
        {
            get
            {
                XElement x = this.GetElement(XName.Get("messageacknowledgment", ""));
                return ((messageacknowledgment)(x));
            }
            set
            {
                this.SetElement(XName.Get("messageacknowledgment", ""), value);
            }
        }

        /// <summary>
        /// <para>
        /// Occurrence: optional, repeating
        /// </para>
        /// <para>
        /// Regular expression: (messageacknowledgment, reportacknowledgment*)
        /// </para>
        /// </summary>
        public IList<reportacknowledgment> reportacknowledgment
        {
            get
            {
                if ((this.reportacknowledgmentField == null))
                {
                    this.reportacknowledgmentField = new XTypedList<reportacknowledgment>(this, LinqToXsdTypeManager.Instance, XName.Get("reportacknowledgment", ""));
                }
                return this.reportacknowledgmentField;
            }
            set
            {
                if ((value == null))
                {
                    this.reportacknowledgmentField = null;
                }
                else
                {
                    if ((this.reportacknowledgmentField == null))
                    {
                        this.reportacknowledgmentField = XTypedList<reportacknowledgment>.Initialize(this, LinqToXsdTypeManager.Instance, value, XName.Get("reportacknowledgment", ""));
                    }
                    else
                    {
                        XTypedServices.SetList<reportacknowledgment>(this.reportacknowledgmentField, value);
                    }
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Dictionary<XName, System.Type> IXMetaData.LocalElementsDictionary
        {
            get
            {
                return localElementDictionary;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        XName IXMetaData.SchemaName
        {
            get
            {
                return XName.Get("acknowledgment", "");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SchemaOrigin IXMetaData.TypeOrigin
        {
            get
            {
                return SchemaOrigin.Element;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILinqToXsdTypeManager IXMetaData.TypeManager
        {
            get
            {
                return LinqToXsdTypeManager.Instance;
            }
        }

        public void Save(string xmlFile)
        {
            XTypedServices.Save(xmlFile, Untyped);
        }

        public void Save(System.IO.TextWriter tw)
        {
            XTypedServices.Save(tw, Untyped);
        }

        public void Save(System.Xml.XmlWriter xmlWriter)
        {
            XTypedServices.Save(xmlWriter, Untyped);
        }

        public static acknowledgment Load(string xmlFile)
        {
            return XTypedServices.Load<acknowledgment>(xmlFile);
        }

        public static acknowledgment Load(System.IO.TextReader xmlFile)
        {
            return XTypedServices.Load<acknowledgment>(xmlFile);
        }

        public static acknowledgment Parse(string xml)
        {
            return XTypedServices.Parse<acknowledgment>(xml);
        }

        public override XTypedElement Clone()
        {
            return XTypedServices.CloneXTypedElement<acknowledgment>(this);
        }

        private static void BuildElementDictionary()
        {
            localElementDictionary.Add(XName.Get("messageacknowledgment", ""), typeof(messageacknowledgment));
            localElementDictionary.Add(XName.Get("reportacknowledgment", ""), typeof(reportacknowledgment));
        }

        ContentModelEntity IXMetaData.GetContentModel()
        {
            return contentModel;
        }
    }

    public class LinqToXsdTypeManager : ILinqToXsdTypeManager
    {

        static Dictionary<XName, System.Type> elementDictionary = new Dictionary<XName, System.Type>();

        private static XmlSchemaSet schemaSet;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static LinqToXsdTypeManager typeManagerSingleton = new LinqToXsdTypeManager();

        static LinqToXsdTypeManager()
        {
            BuildElementDictionary();
        }

        XmlSchemaSet ILinqToXsdTypeManager.Schemas
        {
            get
            {
                if ((schemaSet == null))
                {
                    XmlSchemaSet tempSet = new XmlSchemaSet();
                    System.Threading.Interlocked.CompareExchange(ref schemaSet, tempSet, null);
                }
                return schemaSet;
            }
            set
            {
                schemaSet = value;
            }
        }

        Dictionary<XName, System.Type> ILinqToXsdTypeManager.GlobalTypeDictionary
        {
            get
            {
                return XTypedServices.EmptyDictionary;
            }
        }

        Dictionary<XName, System.Type> ILinqToXsdTypeManager.GlobalElementDictionary
        {
            get
            {
                return elementDictionary;
            }
        }

        Dictionary<System.Type, System.Type> ILinqToXsdTypeManager.RootContentTypeMapping
        {
            get
            {
                return XTypedServices.EmptyTypeMappingDictionary;
            }
        }

        public static LinqToXsdTypeManager Instance
        {
            get
            {
                return typeManagerSingleton;
            }
        }

        private static void BuildElementDictionary()
        {
            elementDictionary.Add(XName.Get("evprmack", ""), typeof(EVMessage.Acknowledgement.evprmack));
            elementDictionary.Add(XName.Get("ichicsrmessageheader", ""), typeof(EVMessage.Acknowledgement.ichicsrmessageheader));
            elementDictionary.Add(XName.Get("messageacknowledgment", ""), typeof(EVMessage.Acknowledgement.messageacknowledgment));
            elementDictionary.Add(XName.Get("reportacknowledgment", ""), typeof(EVMessage.Acknowledgement.reportacknowledgment));
            elementDictionary.Add(XName.Get("messagetype", ""), typeof(EVMessage.Acknowledgement.messagetype));
            elementDictionary.Add(XName.Get("messageformatversion", ""), typeof(EVMessage.Acknowledgement.messageformatversion));
            elementDictionary.Add(XName.Get("messageformatrelease", ""), typeof(EVMessage.Acknowledgement.messageformatrelease));
            elementDictionary.Add(XName.Get("messagenumb", ""), typeof(EVMessage.Acknowledgement.messagenumb));
            elementDictionary.Add(XName.Get("messagesenderidentifier", ""), typeof(EVMessage.Acknowledgement.messagesenderidentifier));
            elementDictionary.Add(XName.Get("messagereceiveridentifier", ""), typeof(EVMessage.Acknowledgement.messagereceiveridentifier));
            elementDictionary.Add(XName.Get("messagedateformat", ""), typeof(EVMessage.Acknowledgement.messagedateformat));
            elementDictionary.Add(XName.Get("messagedate", ""), typeof(EVMessage.Acknowledgement.messagedate));
            elementDictionary.Add(XName.Get("acknowledgment", ""), typeof(EVMessage.Acknowledgement.acknowledgment));
        }

        protected internal static void AddSchemas(XmlSchemaSet schemas)
        {
            schemas.Add(schemaSet);
        }

        public static System.Type GetRootType()
        {
            return elementDictionary[XName.Get("evprmack", "")];
        }
    }

    public partial class XRootNamespace
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private XDocument doc;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private XTypedElement rootObject;


        public evprmack evprmack { get { return rootObject as evprmack; } }

        public ichicsrmessageheader ichicsrmessageheader { get { return rootObject as ichicsrmessageheader; } }

        public messageacknowledgment messageacknowledgment { get { return rootObject as messageacknowledgment; } }

        public reportacknowledgment reportacknowledgment { get { return rootObject as reportacknowledgment; } }

        public messagetype messagetype { get { return rootObject as messagetype; } }

        public messageformatversion messageformatversion { get { return rootObject as messageformatversion; } }

        public messageformatrelease messageformatrelease { get { return rootObject as messageformatrelease; } }

        public messagenumb messagenumb { get { return rootObject as messagenumb; } }

        public messagesenderidentifier messagesenderidentifier { get { return rootObject as messagesenderidentifier; } }

        public messagereceiveridentifier messagereceiveridentifier { get { return rootObject as messagereceiveridentifier; } }

        public messagedateformat messagedateformat { get { return rootObject as messagedateformat; } }

        public messagedate messagedate { get { return rootObject as messagedate; } }

        public acknowledgment acknowledgment { get { return rootObject as acknowledgment; } }

        private XRootNamespace()
        {
        }

        public XRootNamespace(evprmack root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(ichicsrmessageheader root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(messageacknowledgment root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(reportacknowledgment root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(messagetype root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(messageformatversion root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(messageformatrelease root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(messagenumb root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(messagesenderidentifier root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(messagereceiveridentifier root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(messagedateformat root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(messagedate root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRootNamespace(acknowledgment root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XDocument XDocument
        {
            get
            {
                return doc;
            }
        }

        public static XRootNamespace Load(string xmlFile)
        {
            XRootNamespace root = new XRootNamespace();
            root.doc = XDocument.Load(xmlFile);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRootNamespace Load(string xmlFile, LoadOptions options)
        {
            XRootNamespace root = new XRootNamespace();
            root.doc = XDocument.Load(xmlFile, options);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRootNamespace Load(TextReader textReader)
        {
            XRootNamespace root = new XRootNamespace();
            root.doc = XDocument.Load(textReader);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRootNamespace Load(TextReader textReader, LoadOptions options)
        {
            XRootNamespace root = new XRootNamespace();
            root.doc = XDocument.Load(textReader, options);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRootNamespace Load(XmlReader xmlReader)
        {
            XRootNamespace root = new XRootNamespace();
            root.doc = XDocument.Load(xmlReader);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRootNamespace Parse(string text)
        {
            XRootNamespace root = new XRootNamespace();
            root.doc = XDocument.Parse(text);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRootNamespace Parse(string text, LoadOptions options)
        {
            XRootNamespace root = new XRootNamespace();
            root.doc = XDocument.Parse(text, options);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public virtual void Save(string fileName)
        {
            doc.Save(fileName);
        }

        public virtual void Save(TextWriter textWriter)
        {
            doc.Save(textWriter);
        }

        public virtual void Save(XmlWriter writer)
        {
            doc.Save(writer);
        }

        public virtual void Save(TextWriter textWriter, SaveOptions options)
        {
            doc.Save(textWriter, options);
        }

        public virtual void Save(string fileName, SaveOptions options)
        {
            doc.Save(fileName, options);
        }
    }

    public partial class XRoot
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private XDocument doc;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private XTypedElement rootObject;


        public evprmack evprmack { get { return rootObject as evprmack; } }

        public ichicsrmessageheader ichicsrmessageheader { get { return rootObject as ichicsrmessageheader; } }

        public messageacknowledgment messageacknowledgment { get { return rootObject as messageacknowledgment; } }

        public reportacknowledgment reportacknowledgment { get { return rootObject as reportacknowledgment; } }

        public messagetype messagetype { get { return rootObject as messagetype; } }

        public messageformatversion messageformatversion { get { return rootObject as messageformatversion; } }

        public messageformatrelease messageformatrelease { get { return rootObject as messageformatrelease; } }

        public messagenumb messagenumb { get { return rootObject as messagenumb; } }

        public messagesenderidentifier messagesenderidentifier { get { return rootObject as messagesenderidentifier; } }

        public messagereceiveridentifier messagereceiveridentifier { get { return rootObject as messagereceiveridentifier; } }

        public messagedateformat messagedateformat { get { return rootObject as messagedateformat; } }

        public messagedate messagedate { get { return rootObject as messagedate; } }

        public acknowledgment acknowledgment { get { return rootObject as acknowledgment; } }

        private XRoot()
        {
        }

        public XRoot(evprmack root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(ichicsrmessageheader root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(messageacknowledgment root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(reportacknowledgment root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(messagetype root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(messageformatversion root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(messageformatrelease root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(messagenumb root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(messagesenderidentifier root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(messagereceiveridentifier root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(messagedateformat root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(messagedate root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XRoot(acknowledgment root)
        {
            this.doc = new XDocument(root.Untyped);
            this.rootObject = root;
        }

        public XDocument XDocument
        {
            get
            {
                return doc;
            }
        }

        public static XRoot Load(string xmlFile)
        {
            XRoot root = new XRoot();
            root.doc = XDocument.Load(xmlFile);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRoot Load(string xmlFile, LoadOptions options)
        {
            XRoot root = new XRoot();
            root.doc = XDocument.Load(xmlFile, options);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRoot Load(TextReader textReader)
        {
            XRoot root = new XRoot();
            root.doc = XDocument.Load(textReader);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRoot Load(TextReader textReader, LoadOptions options)
        {
            XRoot root = new XRoot();
            root.doc = XDocument.Load(textReader, options);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRoot Load(XmlReader xmlReader)
        {
            XRoot root = new XRoot();
            root.doc = XDocument.Load(xmlReader);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRoot Parse(string text)
        {
            XRoot root = new XRoot();
            root.doc = XDocument.Parse(text);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public static XRoot Parse(string text, LoadOptions options)
        {
            XRoot root = new XRoot();
            root.doc = XDocument.Parse(text, options);
            XTypedElement typedRoot = XTypedServices.ToXTypedElement(root.doc.Root, LinqToXsdTypeManager.Instance);
            if ((typedRoot == null))
            {
                throw new LinqToXsdException("Invalid root element in xml document.");
            }
            root.rootObject = typedRoot;
            return root;
        }

        public virtual void Save(string fileName)
        {
            doc.Save(fileName);
        }

        public virtual void Save(TextWriter textWriter)
        {
            doc.Save(textWriter);
        }

        public virtual void Save(XmlWriter writer)
        {
            doc.Save(writer);
        }

        public virtual void Save(TextWriter textWriter, SaveOptions options)
        {
            doc.Save(textWriter, options);
        }

        public virtual void Save(string fileName, SaveOptions options)
        {
            doc.Save(fileName, options);
        }
    }

}

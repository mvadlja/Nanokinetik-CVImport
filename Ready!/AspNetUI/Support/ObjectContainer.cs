using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using GEM2Common;
using CommonTypes;
using Ready.Model;
using System.Drawing;
using System.Data;
using System.Transactions;

namespace AspNetUI.Support
{
    public enum ActionType { NULL, New, Edit, Delete, Load };
    public enum StatusType { NULL, Temp, Saved };

    [Serializable]
    public class ObjectContainer
    {
        object _object;
        ObjectContainer _editedObjectContainer;
        Dictionary<string, List<ObjectContainer>> _assignedObjects;
        Dictionary<string, List<ObjectContainer>> _assignedObjectsTemp;
        ActionType _action;
        StatusType _status;
        ActionType _actionOld;
        StatusType _statusOld;
        String _type;
        int _id;
        bool _inDB;

        public ObjectContainer() 
        {
            _assignedObjects = new Dictionary<string, List<ObjectContainer>>();
            _assignedObjectsTemp = new Dictionary<string, List<ObjectContainer>>();
            Action = ActionType.NULL;
            Status = StatusType.NULL;
            ActionOld = ActionType.NULL;
            StatusOld = StatusType.NULL;
            _inDB = false;
        }

        public object Object
        {
          get
          {
            return _object;
          }
          set
          {
            if (_object == value)
              return;
            _object = value;
          }
        }
        public ObjectContainer EditedObjectContainer
        {
            get
            {
                return _editedObjectContainer;
            }
            set
            {
                if (_editedObjectContainer == value)
                    return;
                _editedObjectContainer = value;
            }
        }
        public Dictionary<string, List<ObjectContainer>> AssignedObjects
        {
          get
          {
            return _assignedObjects;
          }
          set
          {
            if (_assignedObjects == value)
              return;
            _assignedObjects = value;
          }
        }
        public Dictionary<string, List<ObjectContainer>> AssignedObjectsTemp
        {
          get
          {
            return _assignedObjectsTemp;
          }
          set
          {
            if (_assignedObjectsTemp == value)
              return;
            _assignedObjectsTemp = value;
          }
        }
        public ActionType Action
        {
          get
          {
            return _action;
          }
          set
          {
            if (_action == value)
              return;
            _action = value;
          }
        }
        public StatusType Status
        {
          get
          {
            return _status;
          }
          set
          {
            if (_status == value)
              return;
            _status = value;
          }
        }
        public ActionType ActionOld
        {
          get
          {
            return _actionOld;
          }
          set
          {
            if (_actionOld == value)
              return;
            _actionOld = value;
          }
        }
        public StatusType StatusOld
        {
            get
            {
                return _statusOld;
            }
            set
            {
                if (_statusOld == value)
                    return;
                _statusOld = value;
            }
        }
        public int ID
        {
          get
          {
            return _id;
          }
          set
          {
            if (_id == value)
              return;
            _id = value;
          }
        }

        public bool InDB
        {
            get
            {
                return _inDB;
            }
            set
            {
                if (_inDB == value)
                    return;
                _inDB = value;
            }
        }
        public String Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type == value)
                    return;
                _type = value;
            }
        }
        public void SetState(ActionType inAction, StatusType inStatus)
        {
            ActionOld = Action;
            StatusOld = Status;
            Action = inAction;
            Status = inStatus;
        }

        public void RestoreState()
        {
            Action = ActionOld;
            Status = StatusOld;
            ActionOld = ActionType.NULL;
            StatusOld = StatusType.NULL;
        }

        public void SetLoadedFromDB()
        {
            SetState(ActionType.Load, StatusType.Saved);
            InDB = true;
        }
    }
}
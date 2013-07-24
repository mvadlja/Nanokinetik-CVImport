using System;
using System.Collections.Generic;
using Ready.Model;
using System.Transactions;

namespace AspNetUI.Support
{
    [Serializable]
    public class SSIRepository
    {
        #region Declarations
        private static SSIRepository _instance = null;

        Dictionary<string, List<ObjectContainer>> _assignedObjects;
        Dictionary<string, List<ObjectContainer>> _assignedObjectsTemp;
        Dictionary<string, int> _objectHighestID;

        public Dictionary<string, List<ObjectContainer>> AssignedObjects
        {
            get { return _assignedObjects; }
        }
        public Dictionary<string, List<ObjectContainer>> AssignedObjectsTemp
        {
            get { return _assignedObjectsTemp; }
        }
        #endregion

        #region SSIRep Functions

        public SSIRepository()
        {
            _assignedObjects = new Dictionary<string, List<ObjectContainer>>();
            _assignedObjectsTemp = new Dictionary<string, List<ObjectContainer>>();

            _objectHighestID = new Dictionary<string, int>();
            _objectHighestID.Add("SubstanceCode", 1);
            _objectHighestID.Add("OfficialName", 1);
            _objectHighestID.Add("SubstanceName", 1);
            _objectHighestID.Add("ReferenceSource", 1);
            _objectHighestID.Add("Structure", 1);
            _objectHighestID.Add("Isotope", 1);
            _objectHighestID.Add("Subtype", 1);
            _objectHighestID.Add("SubstanceClassification", 1);
            _objectHighestID.Add("SubstanceRelationship", 1);
            _objectHighestID.Add("Gene", 1);
            _objectHighestID.Add("GeneElement", 1);
            _objectHighestID.Add("Target", 1);
            _objectHighestID.Add("Version", 1);
            _objectHighestID.Add("Moiety", 1);
            _objectHighestID.Add("Amount", 1);
            _objectHighestID.Add("Property", 1);
            _objectHighestID.Add("Domain", 1);
            _objectHighestID.Add("Juristiction", 1);
            _objectHighestID.Add("ReferenceInformation", 1);
            _objectHighestID.Add("SubstanceTranslation", 1);
            _objectHighestID.Add("SubstanceAttachment", 1);
            _objectHighestID.Add("SubstanceAlias", 1);
            _objectHighestID.Add("InternationalCode", 1);
            _objectHighestID.Add("PreviousEvcode", 1);
            _objectHighestID.Add("AliasTranslation", 1);
            _objectHighestID.Add("Sing", 1);
            _objectHighestID.Add("Chemical", 1);
            _objectHighestID.Add("NonStoichiometric", 1);

        }

        public void Reset()
        {
            this._assignedObjects.Clear();
            this._assignedObjectsTemp.Clear();
            this._objectHighestID.Clear();

            _objectHighestID.Add("SubstanceCode", 1);
            _objectHighestID.Add("SubstanceName", 1);
            _objectHighestID.Add("OfficialName", 1);
            _objectHighestID.Add("ReferenceSource", 1);
            _objectHighestID.Add("Structure", 1);
            _objectHighestID.Add("Isotope", 1);
            _objectHighestID.Add("Subtype", 1);
            _objectHighestID.Add("SubstanceClassification", 1);
            _objectHighestID.Add("SubstanceRelationship", 1);
            _objectHighestID.Add("Gene", 1);
            _objectHighestID.Add("GeneElement", 1);
            _objectHighestID.Add("Target", 1);
            _objectHighestID.Add("Version", 1);
            _objectHighestID.Add("Moiety", 1);
            _objectHighestID.Add("Amount", 1);
            _objectHighestID.Add("Property", 1);
            _objectHighestID.Add("Domain", 1);
            _objectHighestID.Add("Juristiction", 1);
            _objectHighestID.Add("ReferenceInformation", 1);
            _objectHighestID.Add("SubstanceTranslation", 1);
            _objectHighestID.Add("SubstanceAttachment", 1);
            _objectHighestID.Add("SubstanceAlias", 1);
            _objectHighestID.Add("InternationalCode", 1);
            _objectHighestID.Add("PreviousEvcode", 1);
            _objectHighestID.Add("AliasTranslation", 1);
            _objectHighestID.Add("Sing", 1);
            _objectHighestID.Add("Chemical", 1);
            _objectHighestID.Add("NonStoichiometric", 1);

        }

        public ObjectContainer AddObject(int inID, object inObject, string inType, ObjectContainer inParent)
        {
            ObjectContainer newObject = new ObjectContainer();
            newObject.Object = inObject;
            newObject.ID = inID;

            if (newObject.ID >= _objectHighestID[inType])
                _objectHighestID[inType] = newObject.ID + 1;

            if (inParent == null)
            {
                if (!this.AssignedObjects.ContainsKey(inType))
                    this.AssignedObjects.Add(inType, new List<ObjectContainer>());

                if (!this.AssignedObjectsTemp.ContainsKey(inType))
                    this.AssignedObjectsTemp.Add(inType, new List<ObjectContainer>());

                this.AssignedObjects[inType].Add(newObject);
                this.AssignedObjectsTemp[inType].Add(newObject);
            }
            else
            {

                if (!inParent.AssignedObjects.ContainsKey(inType))
                    inParent.AssignedObjects.Add(inType, new List<ObjectContainer>());

                if (!inParent.AssignedObjectsTemp.ContainsKey(inType))
                    inParent.AssignedObjectsTemp.Add(inType, new List<ObjectContainer>());

                inParent.AssignedObjects[inType].Add(newObject);
                inParent.AssignedObjectsTemp[inType].Add(newObject);

            }
            return newObject;
        }

        public void DeleteObjectByID(int inID, string inType, ObjectContainer inParent)
        {
            ObjectContainer oc = GetObjectContainer(GetNotDeletedObjectByID(inID, inType, inParent), inType, inParent);
            oc.SetState(ActionType.Delete, StatusType.Saved);

        }

        public void DeleteAssociatedObjects(ObjectContainer inObjectOC)
        {
            if (inObjectOC == null)
            {
                if (this.AssignedObjects.Count == 0) return;

                foreach (string key in this.AssignedObjects.Keys)
                    foreach (ObjectContainer oc in this.AssignedObjects[key])
                    {
                        oc.SetState(ActionType.Delete, StatusType.Saved);
                        DeleteAssociatedObjects(oc);
                    }
            }
            else
            {
                if (inObjectOC.AssignedObjects.Count == 0) return;

                foreach (string key in inObjectOC.AssignedObjects.Keys)
                    foreach (ObjectContainer oc in inObjectOC.AssignedObjects[key])
                    {
                        oc.SetState(ActionType.Delete, StatusType.Saved);
                        DeleteAssociatedObjects(oc);
                    }
            }
        }

        public int ObjectHighestID(string inType)
        {
            return _objectHighestID[inType];
        }

        public ObjectContainer GetObjectContainer(object inObject, string inType, ObjectContainer inParent)
        {
            if (inParent == null)
            {
                if (AssignedObjects.ContainsKey(inType) &&
                    AssignedObjects[inType].Count > 0)
                    foreach (ObjectContainer obj in AssignedObjects[inType])
                    {
                        if (obj.Object.Equals(inObject)) return obj;
                    }
            }
            else
            {
                if (inParent.AssignedObjects.ContainsKey(inType) &&
                    inParent.AssignedObjects[inType].Count > 0)
                    foreach (ObjectContainer obj in inParent.AssignedObjects[inType])
                    {
                        if (obj.Object.Equals(inObject)) return obj;
                    }
            }

            return null;
        }

        public object GetObjectByID(int inID, string inType, ObjectContainer inParent)
        {
            if (inParent == null)
            {
                if (AssignedObjects.ContainsKey(inType) &&
                    AssignedObjects[inType].Count > 0)
                    foreach (ObjectContainer obj in AssignedObjects[inType])
                    {
                        if (obj.ID.Equals(inID)) return obj.Object;
                    }
            }
            else
            {
                if (inParent.AssignedObjects.ContainsKey(inType) &&
                    inParent.AssignedObjects[inType].Count > 0)
                    foreach (ObjectContainer obj in inParent.AssignedObjects[inType])
                    {
                        if (obj.ID.Equals(inID))
                            return obj.Object;
                    }
            }
            return null;
        }

        public object GetNotDeletedObjectByID(int inID, string inType, ObjectContainer inParent)
        {
            if (inParent == null)
            {
                if (AssignedObjects.ContainsKey(inType) &&
                    AssignedObjects[inType].Count > 0)
                    foreach (ObjectContainer obj in AssignedObjects[inType])
                    {
                        if (obj.ID.Equals(inID) && obj.Action != ActionType.Delete) return obj.Object;
                    }
            }
            else
            {
                if (inParent.AssignedObjects.ContainsKey(inType) &&
                    inParent.AssignedObjects[inType].Count > 0)
                    foreach (ObjectContainer obj in inParent.AssignedObjects[inType])
                    {
                        if (obj.ID.Equals(inID) && obj.Action != ActionType.Delete)
                            return obj.Object;
                    }
            }
            return null;
        }

        public List<T> GetObjectsList<T>(string inType, ObjectContainer inParent)
        {
            List<T> outList = new List<T>();

            if (inParent == null)
            {
                if (this.AssignedObjects.ContainsKey(inType) &&
                    this.AssignedObjects[inType].Count > 0)
                    foreach (ObjectContainer obj in this.AssignedObjects[inType])
                        if (obj.Action != ActionType.Delete)
                            outList.Add((T)obj.Object);
            }
            else
            {
                if (inParent.AssignedObjects.ContainsKey(inType) &&
                    inParent.AssignedObjects[inType].Count > 0)
                    foreach (ObjectContainer obj in inParent.AssignedObjects[inType])
                        if (obj.Action != ActionType.Delete)
                            outList.Add((T)obj.Object);
            }
            return outList;
        }

        public void SaveState(ObjectContainer inOC, string inType, ObjectContainer inParent)
        {

            if (inParent == null)
            {
                if (inOC.Action == ActionType.Edit && inOC.Status == StatusType.Temp)
                {
                    inOC.SetState(ActionType.Edit, StatusType.Saved);
                    inOC.EditedObjectContainer = null;
                }
                if (inOC.Action == ActionType.New && inOC.Status == StatusType.Temp)
                {
                    inOC.SetState(ActionType.New, StatusType.Saved);
                    inOC.EditedObjectContainer = null;
                }
                List<ObjectContainer> ocToDelete = new List<ObjectContainer>();
                foreach (ObjectContainer oc in AssignedObjects[inType])
                {
                    if (oc.Action == ActionType.Delete && oc.Status == StatusType.Temp)
                        ocToDelete.Add(oc);
                }
                foreach (ObjectContainer oc in ocToDelete)
                    AssignedObjects[inType].Remove(oc);
                AssignedObjectsTemp[inType].Clear();
                DeleteOldAndUpdateNewObjects(inOC);
            }
            else if (inParent.Type == "ReferenceInformation" || inParent.Type == "NonStoichiometric")
            {
                
                if (inOC.Action == ActionType.Edit && inOC.Status == StatusType.Temp)
                {
                    inOC.SetState(ActionType.Edit, StatusType.Saved);
                    inOC.EditedObjectContainer = null;
                }
                if (inOC.Action == ActionType.New && inOC.Status == StatusType.Temp)
                {
                    inOC.SetState(ActionType.New, StatusType.Saved);
                    inOC.EditedObjectContainer = null;
                }
                List<ObjectContainer> ocToDelete = new List<ObjectContainer>();
                foreach (ObjectContainer oc in inParent.AssignedObjects[inType])
                {
                    if (oc.Action == ActionType.Delete && oc.Status == StatusType.Temp)
                        ocToDelete.Add(oc);
                }
                foreach (ObjectContainer oc in ocToDelete)
                    inParent.AssignedObjects[inType].Remove(oc);
                inParent.AssignedObjectsTemp[inType].Clear();
                DeleteOldAndUpdateNewObjects(inOC);
            }
        }
        private void DeleteOldAndUpdateNewObjects(ObjectContainer inObjectOC)
        {

            foreach (string key in inObjectOC.AssignedObjects.Keys)
            {
                List<ObjectContainer> ocToDelete = new List<ObjectContainer>();
                foreach (ObjectContainer oc in inObjectOC.AssignedObjects[key])
                {
                    if (oc.Action == ActionType.Delete && oc.Status == StatusType.Temp)
                        ocToDelete.Add(oc);
                    else
                    {
                        DeleteOldAndUpdateNewObjects(oc);
                        if (oc.Action == ActionType.Edit && oc.Status == StatusType.Temp)
                        {
                            oc.SetState(ActionType.Edit, StatusType.Saved);
                            oc.EditedObjectContainer = null;
                        }
                        if (oc.Action == ActionType.New && oc.Status == StatusType.Temp)
                        {
                            oc.SetState(ActionType.New, StatusType.Saved);
                            oc.EditedObjectContainer = null;
                        }
                    }
                }
                foreach (ObjectContainer oc in ocToDelete)
                    inObjectOC.AssignedObjects[key].Remove(oc);
                if (this.AssignedObjectsTemp.ContainsKey(key))
                    inObjectOC.AssignedObjectsTemp[key].Clear();
            }


        }

        public void DiscardObjectAndRestore(ObjectContainer inObjectOC, string inType, ObjectContainer inParent)
        {
            if (inParent == null)
            {
                if (this.AssignedObjects.ContainsKey(inType) &&
                    this.AssignedObjects[inType].Count > 0)
                {
                    DiscardAssignedObjectsChanges(inObjectOC);
                    RestoreObjectsStates(inObjectOC);
                    foreach (ObjectContainer oc in AssignedObjects[inType])
                        if (oc.EditedObjectContainer == null)
                            oc.RestoreState();
                    this.AssignedObjects[inType].Remove(inObjectOC);
                    this.AssignedObjectsTemp[inType].Remove(inObjectOC);
                }
            }
            else
            {
                if (inParent.AssignedObjects.ContainsKey(inType) &&
                    inParent.AssignedObjects[inType].Count > 0)
                {
                    DiscardAssignedObjectsChanges(inObjectOC);
                    RestoreObjectsStates(inObjectOC);
                    foreach (ObjectContainer oc in inParent.AssignedObjects[inType])
                        if (oc.EditedObjectContainer == null && oc.Action == ActionType.Delete
                            && oc.Status == StatusType.Temp)
                            oc.RestoreState();
                    inParent.AssignedObjects[inType].Remove(inObjectOC);
                    inParent.AssignedObjectsTemp[inType].Remove(inObjectOC);
                }
            }

        }

        private void DiscardAssignedObjectsChanges(ObjectContainer inObjectOC)
        {
            foreach (string key in inObjectOC.AssignedObjectsTemp.Keys)
            {
                foreach (ObjectContainer oc in inObjectOC.AssignedObjectsTemp[key])
                {
                    DiscardAssignedObjectsChanges(oc);
                    inObjectOC.AssignedObjects[key].Remove(oc);
                }
                inObjectOC.AssignedObjectsTemp[key].Clear();
            }
        }

        private void RestoreObjectsStates(ObjectContainer inObjectOC)
        {
            foreach (string key in inObjectOC.AssignedObjects.Keys)
                foreach (ObjectContainer oc in inObjectOC.AssignedObjects[key])
                {
                    if (oc.Action == ActionType.Delete)// && oc.Status == StatusType.Temp)
                        oc.RestoreState();
                    RestoreObjectsStates(oc);
                }
        }
        #endregion

        #region Save to database

        #region SaveAS
        public void SaveASToDb(int idAppSub)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SaveSubstanceTranslation(idAppSub);
                    SaveSubstanceAlias(idAppSub);
                    SaveInternationalCodes(idAppSub);
                    PreviousEvcode(idAppSub);
                    SubstanceAttachment(idAppSub);


                    ts.Complete();
                }
                catch (Exception)
                {
                    return;
                }

            }
        }

        private void SaveSubstanceTranslation(int idAppSub)
        {
            if (!AssignedObjects.ContainsKey("SubstanceTranslation")) return;

            ISubstance_translations_PKOperations _substanceTranslationOperations = new Substance_translations_PKDAL();
            IApproved_subst_subst_trans_PKOperations _aSubTranOperations = new Approved_subst_subst_trans_PKDAL();

            foreach (var subTran in AssignedObjects["SubstanceTranslation"])
            {
                if (subTran.Action != ActionType.Delete)
                {
                    if (subTran.Action == ActionType.New)
                    {
                        (subTran.Object as Substance_translations_PK).substance_translations_PK = null;
                        subTran.Object = _substanceTranslationOperations.Save(subTran.Object as Substance_translations_PK);
                        subTran.ID = (int)(subTran.Object as Substance_translations_PK).substance_translations_PK;

                        Approved_subst_subst_trans_PK aSubTran = new Approved_subst_subst_trans_PK()
                        {
                            approved_substance_FK = idAppSub,
                            substance_translations_FK = (int)(subTran.Object as Substance_translations_PK).substance_translations_PK
                        };
                        _aSubTranOperations.Save(aSubTran);
                    }
                    else if (subTran.Action == ActionType.Edit)
                    {
                        subTran.Object = _substanceTranslationOperations.Save(subTran.Object as Substance_translations_PK);
                    }
                }
                else if (subTran.InDB)
                {
                    _substanceTranslationOperations.Delete(subTran.ID);
                }
            }
        }

        private void SaveSubstanceAlias(int idAppSub)
        {
            if (!AssignedObjects.ContainsKey("SubstanceAlias")) return;
            ISubstance_alias_PKOperations _subAliasOperations = new Substance_alias_PKDAL();
            IApproved_substance_subst_alias_PKOperations _approvedSubAliasOperation = new Approved_substance_subst_alias_PKDAL();
            ISubstance_alias_translation_PKOperations _aliasTranslationOperation = new Substance_alias_translation_PKDAL();
            ISub_alias_sub_alias_tran_PKOperations _subAliasSubTranOperations = new Sub_alias_sub_alias_tran_PKDAL();

            foreach (var subAlias in AssignedObjects["SubstanceAlias"])
            {
                if (subAlias.Action == ActionType.Delete)
                    DeleteAssociatedObjects(subAlias);

                if (subAlias.Action != ActionType.Delete)
                {
                    if (subAlias.Action == ActionType.New)
                    {
                        (subAlias.Object as Substance_alias_PK).substance_alias_PK = null;
                        subAlias.Object = _subAliasOperations.Save(subAlias.Object as Substance_alias_PK);
                        subAlias.ID = (int)(subAlias.Object as Substance_alias_PK).substance_alias_PK;

                        Approved_substance_subst_alias_PK aAlias = new Approved_substance_subst_alias_PK()
                        {
                            approved_substance_FK = idAppSub,
                            substance_alias_FK = subAlias.ID
                        };
                        _approvedSubAliasOperation.Save(aAlias);
                    }
                    else if (subAlias.Action == ActionType.Edit)
                    {
                        subAlias.Object = _subAliasOperations.Save(subAlias.Object as Substance_alias_PK);
                    }

                    if (subAlias.AssignedObjects.ContainsKey("AliasTranslation"))
                    {
                        foreach (var aliasTranslation in subAlias.AssignedObjects["AliasTranslation"])
                        {
                            if (aliasTranslation.Action != ActionType.Delete)
                            {
                                if (aliasTranslation.Action == ActionType.New)
                                {
                                    (aliasTranslation.Object as Substance_alias_translation_PK).substance_alias_translation_PK = null;
                                    aliasTranslation.Object = _aliasTranslationOperation.Save(aliasTranslation.Object as Substance_alias_translation_PK);
                                    aliasTranslation.ID = (int)(aliasTranslation.Object as Substance_alias_translation_PK).substance_alias_translation_PK;

                                    Sub_alias_sub_alias_tran_PK aliasTranslationMn = new Sub_alias_sub_alias_tran_PK()
                                    {
                                        sub_alias_FK = subAlias.ID,
                                        sub_alias_tran_FK = aliasTranslation.ID
                                    };
                                    _subAliasSubTranOperations.Save(aliasTranslationMn);
                                }
                                else if (aliasTranslation.Action == ActionType.Edit)
                                {
                                    aliasTranslation.Object = _aliasTranslationOperation.Save(aliasTranslation.Object as Substance_alias_translation_PK);
                                }
                            }
                            else if (aliasTranslation.InDB)
                            {
                                _aliasTranslationOperation.Delete(aliasTranslation.ID);
                            }
                        }
                    }
                }
                else if (subAlias.InDB)
                {
                    _subAliasOperations.Delete(subAlias.ID);
                }
            }
        }

        private void SaveInternationalCodes(int idAppSub)
        {
            if (!AssignedObjects.ContainsKey("InternationalCode")) return;

            IInternational_code_PKOperations _internationalCodeOperatins = new International_code_PKDAL();
            IApproved_subst_inter_code_PKOperations _approvedSubInterCode = new Approved_subst_inter_code_PKDAL();

            foreach (var internationalCode in AssignedObjects["InternationalCode"])
            {
                if (internationalCode.Action != ActionType.Delete)
                {
                    if (internationalCode.Action == ActionType.New)
                    {
                        (internationalCode.Object as International_code_PK).international_code_PK = null;
                        internationalCode.Object = _internationalCodeOperatins.Save(internationalCode.Object as International_code_PK);
                        internationalCode.ID = (int)(internationalCode.Object as International_code_PK).international_code_PK;

                        Approved_subst_inter_code_PK approvedSubInterCode = new Approved_subst_inter_code_PK()
                        {
                            approved_substance_FK = idAppSub,
                            international_code_FK = internationalCode.ID
                        };
                        _approvedSubInterCode.Save(approvedSubInterCode);
                    }
                    else if (internationalCode.Action == ActionType.Edit)
                    {
                        internationalCode.Object = _internationalCodeOperatins.Save(internationalCode.Object as International_code_PK);
                    }
                }
                else if (internationalCode.InDB)
                {
                    _internationalCodeOperatins.Delete(internationalCode.ID);
                }
            }
        }

        private void PreviousEvcode(int idAppSub)
        {
            if (!AssignedObjects.ContainsKey("PreviousEvcode")) return;

            IAs_previous_ev_code_PKOperations _previousEvcodeOperations = new As_previous_ev_code_PKDAL();
            IApproved_subst_prev_ev_code_PKOperations _previousEvcodeASOperations = new Approved_subst_prev_ev_code_PKDAL();
            foreach (var previousEvcode in AssignedObjects["PreviousEvcode"])
            {
                if (previousEvcode.Action != ActionType.Delete)
                {
                    if (previousEvcode.Action == ActionType.New)
                    {
                        (previousEvcode.Object as As_previous_ev_code_PK).as_previous_ev_code_PK = null;
                        previousEvcode.Object = _previousEvcodeOperations.Save(previousEvcode.Object as As_previous_ev_code_PK);
                        previousEvcode.ID = (int)(previousEvcode.Object as As_previous_ev_code_PK).as_previous_ev_code_PK;

                        Approved_subst_prev_ev_code_PK previousEvcodeAS = new Approved_subst_prev_ev_code_PK()
                        {
                            approved_substance_FK = idAppSub,
                            as_previous_ev_code_FK = previousEvcode.ID
                        };
                        _previousEvcodeASOperations.Save(previousEvcodeAS);
                    }
                    else if (previousEvcode.Action == ActionType.Edit)
                    {
                        previousEvcode.Object = _previousEvcodeOperations.Save(previousEvcode.Object as As_previous_ev_code_PK);
                    }
                }
                else if (previousEvcode.InDB)
                {
                    _previousEvcodeOperations.Delete(previousEvcode.ID);
                }
            }
        }

        private void SubstanceAttachment(int idAppSub)
        {
            if (!AssignedObjects.ContainsKey("SubstanceAttachment")) return;

            ISubstance_attachment_PKOperations _substanceAttachment = new Substance_attachment_PKDAL();
            IApproved_subst_subst_att_PKOperations _approvedSubAttachment = new Approved_subst_subst_att_PKDAL();
            foreach (var substanceAttachment in AssignedObjects["SubstanceAttachment"])
            {
                if (substanceAttachment.Action != ActionType.Delete)
                {
                    if (substanceAttachment.Action == ActionType.New)
                    {
                        (substanceAttachment.Object as Substance_attachment_PK).substance_attachment_PK = null;
                        substanceAttachment.Object = _substanceAttachment.Save(substanceAttachment.Object as Substance_attachment_PK);
                        substanceAttachment.ID = (int)(substanceAttachment.Object as Substance_attachment_PK).substance_attachment_PK;

                        Approved_subst_subst_att_PK approvedSubAttachment = new Approved_subst_subst_att_PK()
                        {
                            approved_substance_FK = idAppSub,
                            substance_attachment_FK = substanceAttachment.ID
                        };
                        _approvedSubAttachment.Save(approvedSubAttachment);
                    }
                    else if (substanceAttachment.Action == ActionType.Edit)
                    {
                        substanceAttachment.Object = _substanceAttachment.Save(substanceAttachment.Object as Substance_attachment_PK);
                    }
                }
                else if (substanceAttachment.InDB)
                {
                    _substanceAttachment.Delete(substanceAttachment.ID);
                }
            }
        }

        #endregion

        public void SaveToDb(Substance_s_PK inSubstance)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SaveSubstanceCodeToDb(inSubstance);
                    SaveSubstanceNameToDb(inSubstance);
                    SaveVersionToDb(inSubstance);

                    Reference_info_PK refInfo = GetObjectByID((int)inSubstance.ref_info_FK, "ReferenceInformation", null) as Reference_info_PK;
                    SaveReferenceInformation(refInfo);

                    if (inSubstance.sing_FK != null)
                    {
                        Sing_PK sing = GetObjectByID((int)inSubstance.sing_FK, "Sing", null) as Sing_PK;
                        ObjectContainer ocSing = GetObjectContainer(sing, "Sing", null);
                        if (sing != null)
                        {
                            SaveStructureToDatabase(ocSing);
                            if (sing.chemical_FK != null)
                            {
                                Chemical_PK chemical = GetObjectByID((int)sing.chemical_FK, "Chemical", ocSing) as Chemical_PK;
                                ObjectContainer ocChemical = GetObjectContainer(chemical, "Chemical", ocSing);

                                if (chemical.non_stoichio_FK != null)
                                {

                                    Non_stoichiometric_PK nonSto = GetObjectByID((int)chemical.non_stoichio_FK, "NonStoichiometric", ocChemical) as Non_stoichiometric_PK;
                                    ObjectContainer ocNonSto = GetObjectContainer(nonSto, "NonStoichiometric", ocChemical);

                                    SaveMoietyToDb(ocNonSto);
                                    SavePropertyToDb(ocNonSto);
                                }
                            }
                        }
                    }

                    ts.Complete();
                }
                catch (Exception)
                {
                    return;
                }

            }
        }

        private void SaveReferenceInformation(Reference_info_PK inRefInfo)
        {
            SaveGeneElement(inRefInfo);
            SaveTargetToDb(inRefInfo);
            SaveSubstanceClassification(inRefInfo);
            SaveGeneToDb(inRefInfo);
            SaveSubstanceRelationship(inRefInfo);
        }

        private void SaveSubstanceRelationship(Reference_info_PK inRefInfo)
        {
            ObjectContainer refInfoOC = GetObjectContainer(inRefInfo, "ReferenceInformation", null);
            if (!refInfoOC.AssignedObjects.ContainsKey("SubstanceRelationship")) return;

            ISubstance_relationship_PKOperations _substanceRelationship = new Substance_relationship_PKDAL();
            IReference_source_PKOperations _referenceSource = new Reference_source_PKDAL();
            IAmount_PKOperations _amountOperations = new Amount_PKDAL();
            IRs_sr_mn_PKOperations _refSourceRel = new Rs_sr_mn_PKDAL();
            ISr_ri_mn_PKOperations _sr_ri_mn_PKOperations = new Sr_ri_mn_PKDAL();

            foreach (var subRel in refInfoOC.AssignedObjects["SubstanceRelationship"])
            {
                if (subRel.Action == ActionType.Delete)
                    DeleteAssociatedObjects(subRel);

                if (subRel.AssignedObjects.ContainsKey("ReferenceSource"))
                {
                    foreach (var referenceSource in subRel.AssignedObjects["ReferenceSource"])
                    {
                        if (referenceSource.Action != ActionType.Delete)
                        {
                            if (referenceSource.Action == ActionType.New)
                            {
                                (referenceSource.Object as Reference_source_PK).reference_source_PK = null;
                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                                referenceSource.ID = (int)(referenceSource.Object as Reference_source_PK).reference_source_PK;
                            }
                            else if (referenceSource.Action == ActionType.Edit)
                            {
                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                            }
                        }
                        else if (referenceSource.InDB)
                        {
                            _referenceSource.Delete<int>(referenceSource.ID);
                        }
                    }
                }

                if (subRel.AssignedObjects.ContainsKey("Amount"))
                {
                    if (subRel.AssignedObjects["Amount"][0].Action != ActionType.Delete)
                    {
                        if (subRel.AssignedObjects["Amount"][0].Action == ActionType.New)
                        {
                            (subRel.AssignedObjects["Amount"][0].Object as Amount_PK).amount_PK = null;
                            subRel.AssignedObjects["Amount"][0].Object = _amountOperations.Save(subRel.AssignedObjects["Amount"][0].Object as Amount_PK);
                            subRel.AssignedObjects["Amount"][0].ID = (int)(subRel.AssignedObjects["Amount"][0].Object as Amount_PK).amount_PK;
                        }
                        else if (subRel.Action == ActionType.Edit)
                        {
                            subRel.AssignedObjects["Amount"][0].Object = _amountOperations.Save(subRel.AssignedObjects["Amount"][0].Object as Amount_PK);
                        }

                        (subRel.Object as Substance_relationship_PK).amount_FK = (subRel.AssignedObjects["Amount"][0].Object as Amount_PK).amount_PK;
                        //subRel.Object = _substanceRelationship.Save(subRel.Object as Substance_relationship_PK);
                    }
                    else if (subRel.AssignedObjects["Amount"][0].InDB)
                    {
                        _amountOperations.Delete<int>(subRel.AssignedObjects["Amount"][0].ID);
                    }

                }

                if (subRel.Action != ActionType.Delete)
                {
                    if (subRel.Action == ActionType.New)
                    {
                        (subRel.Object as Substance_relationship_PK).substance_relationship_PK = null;
                        subRel.Object = _substanceRelationship.Save(subRel.Object as Substance_relationship_PK);
                        subRel.ID = (int)(subRel.Object as Substance_relationship_PK).substance_relationship_PK;

                        Sr_ri_mn_PK newSrRi = new Sr_ri_mn_PK()
                        {
                            ri_FK = inRefInfo.reference_info_PK,
                            sr_FK = (subRel.Object as Substance_relationship_PK).substance_relationship_PK
                        };
                        _sr_ri_mn_PKOperations.Save(newSrRi);
                    }
                    else if (subRel.Action == ActionType.Edit)
                    {
                        subRel.Object = _substanceRelationship.Save(subRel.Object as Substance_relationship_PK);
                    }
                }
                else if (subRel.InDB)
                {
                    _substanceRelationship.Delete(subRel.ID);
                }

                foreach (var referenceSource in subRel.AssignedObjects["ReferenceSource"])
                {
                    if (referenceSource.Action == ActionType.New)
                    {
                        Rs_sr_mn_PK newRsSr = new Rs_sr_mn_PK()
                        {
                            rs_FK = (referenceSource.Object as Reference_source_PK).reference_source_PK,
                            sr_FK = (subRel.Object as Substance_relationship_PK).substance_relationship_PK
                        };
                        _refSourceRel.Save(newRsSr);
                    }
                }

            }
        }

        private void SaveGeneToDb(Reference_info_PK inRefInfo)
        {
            ObjectContainer refInfoOC = GetObjectContainer(inRefInfo, "ReferenceInformation", null);
            if (!refInfoOC.AssignedObjects.ContainsKey("Gene"))
                return;

            IReference_source_PKOperations _referenceSource = new Reference_source_PKDAL();
            IGene_PKOperations _gene = new Gene_PKDAL();
            IRs_gene_mn_PKOperations _rs_gn = new Rs_gene_mn_PKDAL();
            IRi_gene_mn_PKOperations _ri_gene_mn_PKOperations = new Ri_gene_mn_PKDAL();


            foreach (var gene in refInfoOC.AssignedObjects["Gene"])
            {
                if (gene.Action == ActionType.Delete)
                    DeleteAssociatedObjects(gene);

                if (gene.AssignedObjects.ContainsKey("ReferenceSource"))
                {
                    foreach (var referenceSource in gene.AssignedObjects["ReferenceSource"])
                    {
                        if (referenceSource.Action != ActionType.Delete)
                        {
                            if (referenceSource.Action == ActionType.New)
                            {
                                (referenceSource.Object as Reference_source_PK).reference_source_PK = null;
                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                                referenceSource.ID = (int)(referenceSource.Object as Reference_source_PK).reference_source_PK;
                            }
                            else if (referenceSource.Action == ActionType.Edit)
                            {

                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                            }
                        }
                        else if (referenceSource.InDB)
                        {
                            _referenceSource.Delete<int>(referenceSource.ID);
                        }
                    }
                }

                if (gene.Action != ActionType.Delete)
                {
                    if (gene.Action == ActionType.New)
                    {
                        (gene.Object as Gene_PK).gene_PK = null;
                        gene.Object = _gene.Save(gene.Object as Gene_PK);
                        gene.ID = (int)(gene.Object as Gene_PK).gene_PK;

                        Ri_gene_mn_PK newRiGe = new Ri_gene_mn_PK()
                        {
                            ri_FK = inRefInfo.reference_info_PK,
                            gene_FK = (gene.Object as Gene_PK).gene_PK
                        };
                        _ri_gene_mn_PKOperations.Save(newRiGe);
                    }
                    else if (gene.Action == ActionType.Edit)
                    {
                        gene.Object = _gene.Save(gene.Object as Gene_PK);
                    }
                }
                else if (gene.InDB)
                {
                    _gene.Delete(gene.ID);
                }

                if (gene.AssignedObjects.ContainsKey("ReferenceSource"))
                {
                    foreach (var referenceSource in gene.AssignedObjects["ReferenceSource"])
                    {
                        if (referenceSource.Action == ActionType.New)
                        {
                            Rs_gene_mn_PK newRsSc = new Rs_gene_mn_PK()
                            {
                                rs_FK = (referenceSource.Object as Reference_source_PK).reference_source_PK,
                                gene_FK = (gene.Object as Gene_PK).gene_PK
                            };
                            _rs_gn.Save(newRsSc);
                        }

                    }
                }


            }
        }

        private void SaveGeneElement(Reference_info_PK inRefInfo)
        {
            ObjectContainer refInfoOC = GetObjectContainer(inRefInfo, "ReferenceInformation", null);
            if (!refInfoOC.AssignedObjects.ContainsKey("GeneElement"))
                return;

            IReference_source_PKOperations _referenceSource = new Reference_source_PKDAL();
            IGene_element_PKOperations _geneElement = new Gene_element_PKDAL();
            IRs_ge_mn_PKOperations _rs_ge = new Rs_ge_mn_PKDAL();
            IRi_ge_mn_PKOperations _ri_ge_mn_PKOperations = new Ri_ge_mn_PKDAL();

            foreach (var geneElement in refInfoOC.AssignedObjects["GeneElement"])
            {
                if (geneElement.Action == ActionType.Delete)
                    DeleteAssociatedObjects(geneElement);

                if (geneElement.AssignedObjects.ContainsKey("ReferenceSource"))
                {
                    foreach (var referenceSource in geneElement.AssignedObjects["ReferenceSource"])
                    {
                        if (referenceSource.Action != ActionType.Delete)
                        {
                            if (referenceSource.Action == ActionType.New)
                            {
                                (referenceSource.Object as Reference_source_PK).reference_source_PK = null;
                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                                referenceSource.ID = (int)(referenceSource.Object as Reference_source_PK).reference_source_PK;
                            }
                            else if (referenceSource.Action == ActionType.Edit)
                            {

                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                            }
                        }
                        else if (referenceSource.InDB)
                        {
                            _referenceSource.Delete<int>(referenceSource.ID);
                        }
                    }
                }

                if (geneElement.Action != ActionType.Delete)
                {
                    if (geneElement.Action == ActionType.New)
                    {
                        (geneElement.Object as Gene_element_PK).gene_element_PK = null;
                        geneElement.Object = _geneElement.Save(geneElement.Object as Gene_element_PK);
                        geneElement.ID = (int)(geneElement.Object as Gene_element_PK).gene_element_PK;

                        Ri_ge_mn_PK newRiGe = new Ri_ge_mn_PK()
                        {
                            ri_FK = inRefInfo.reference_info_PK,
                            ge_FK = (geneElement.Object as Gene_element_PK).gene_element_PK
                        };
                        _ri_ge_mn_PKOperations.Save(newRiGe);
                    }
                    else if (geneElement.Action == ActionType.Edit)
                    {
                        geneElement.Object = _geneElement.Save(geneElement.Object as Gene_element_PK);
                    }
                }
                else if (geneElement.InDB)
                {
                    _geneElement.Delete<int>(geneElement.ID);
                }

                if (geneElement.AssignedObjects.ContainsKey("ReferenceSource"))
                {
                    foreach (var referenceSource in geneElement.AssignedObjects["ReferenceSource"])
                    {
                        if (referenceSource.Action == ActionType.New)
                        {
                            Rs_ge_mn_PK newRsSc = new Rs_ge_mn_PK()
                            {
                                rs_FK = (referenceSource.Object as Reference_source_PK).reference_source_PK,
                                ge_FK = (geneElement.Object as Gene_element_PK).gene_element_PK
                            };
                            _rs_ge.Save(newRsSc);
                        }
                    }
                }

            }
        }

        private void SaveTargetToDb(Reference_info_PK inRefInfo)
        {
            ObjectContainer refInfoOC = GetObjectContainer(inRefInfo, "ReferenceInformation", null);
            if (!refInfoOC.AssignedObjects.ContainsKey("Target"))
                return;
            ITarget_PKOperations _targetOperation = new Target_PKDAL();
            IRi_target_mn_PKOperations _ri_target_mn_PKOperations = new Ri_target_mn_PKDAL();
            IRs_target_mn_PKOperations _rs_target_mn_PKOperations = new Rs_target_mn_PKDAL();
            IReference_source_PKOperations _referenceSource = new Reference_source_PKDAL();

            foreach (var trg in refInfoOC.AssignedObjects["Target"])
            {
                if (trg.Action == ActionType.Delete)
                    DeleteAssociatedObjects(trg);

                if (trg.Action != ActionType.Delete)
                {
                    if (trg.Action == ActionType.New)
                    {
                        (trg.Object as Target_PK).target_PK = null;
                        trg.Object = _targetOperation.Save(trg.Object as Target_PK);
                        trg.ID = (int)(trg.Object as Target_PK).target_PK;

                        Ri_target_mn_PK newRiGe = new Ri_target_mn_PK()
                        {
                            ri_FK = inRefInfo.reference_info_PK,
                            target_FK = (trg.Object as Target_PK).target_PK
                        };
                        _ri_target_mn_PKOperations.Save(newRiGe);
                    }
                    else if (trg.Action == ActionType.Edit)
                    {
                        trg.Object = _targetOperation.Save(trg.Object as Target_PK);
                    }
                }
                else if (trg.InDB)
                {
                    _targetOperation.Delete<int>(trg.ID);
                }

                foreach (var referenceSource in trg.AssignedObjects["ReferenceSource"])
                {
                    if (referenceSource.Action != ActionType.Delete)
                    {
                        if (referenceSource.Action == ActionType.New)
                        {
                            (referenceSource.Object as Reference_source_PK).reference_source_PK = null;
                            referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                            referenceSource.ID = (int)(referenceSource.Object as Reference_source_PK).reference_source_PK;

                            Rs_target_mn_PK rsTargetMn = new Rs_target_mn_PK()
                            {
                                rs_FK = referenceSource.ID,
                                target_FK = trg.ID
                            };
                            _rs_target_mn_PKOperations.Save(rsTargetMn);
                        }
                        else if (referenceSource.Action == ActionType.Edit)
                        {
                            referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                        }
                    }
                    else if (referenceSource.InDB)
                    {
                        DeleteAssociatedObjects(referenceSource);
                        _referenceSource.Delete(referenceSource.ID);
                    }
                }
            }
        }

        private void SaveSubstanceClassification(Reference_info_PK inRefInfo)
        {
            ObjectContainer refInfoOC = GetObjectContainer(inRefInfo, "ReferenceInformation", null);
            if (!refInfoOC.AssignedObjects.ContainsKey("SubstanceClassification"))
                return;

            ISubst_clf_PKOperations _substanceClassification = new Subst_clf_PKDAL();
            ISubtype_PKOperations _subtype = new Subtype_PKDAL();
            IReference_source_PKOperations _referenceSource = new Reference_source_PKDAL();
            ISubtype_sclf_mn_PKOperations _subtypeSclf = new Subtype_sclf_mn_PKDAL();
            IRs_sclf_mn_PKOperations _refSourceSclf = new Rs_sclf_mn_PKDAL();
            IRi_sclf_mn_PKOperations _ri_sclf_mn_PKOperations = new Ri_sclf_mn_PKDAL();

            foreach (var sclf in refInfoOC.AssignedObjects["SubstanceClassification"])
            {
                if (sclf.Action == ActionType.Delete)
                    DeleteAssociatedObjects(sclf);

                if (sclf.AssignedObjects.ContainsKey("ReferenceSource"))
                {
                    foreach (var referenceSource in sclf.AssignedObjects["ReferenceSource"])
                    {
                        if (referenceSource.Action != ActionType.Delete)
                        {
                            if (referenceSource.Action == ActionType.New)
                            {
                                (referenceSource.Object as Reference_source_PK).reference_source_PK = null;
                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                                referenceSource.ID = (int)(referenceSource.Object as Reference_source_PK).reference_source_PK;
                            }
                            else if (referenceSource.Action == ActionType.Edit)
                            {

                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                            }
                        }
                        else if (referenceSource.InDB)
                        {
                            _referenceSource.Delete<int>(referenceSource.ID);
                        }
                    }
                }

                if (sclf.AssignedObjects.ContainsKey("Subtype"))
                {
                    foreach (var subtype in sclf.AssignedObjects["Subtype"])
                    {
                        if (subtype.Action != ActionType.Delete)
                        {
                            if (subtype.Action == ActionType.New)
                            {
                                (subtype.Object as Subtype_PK).subtype_PK = null;
                                subtype.Object = _subtype.Save(subtype.Object as Subtype_PK);
                                subtype.ID = (int)(subtype.Object as Subtype_PK).subtype_PK;
                            }
                            else if (subtype.Action == ActionType.Edit)
                            {
                                subtype.Object = _subtype.Save(subtype.Object as Subtype_PK);
                            }
                        }

                        else if (subtype.InDB)
                        {
                            _subtype.Delete<int>(subtype.ID);
                        }
                    }
                }

                if (sclf.Action != ActionType.Delete)
                {
                    if (sclf.Action == ActionType.New)
                    {
                        (sclf.Object as Subst_clf_PK).subst_clf_PK = null;
                        sclf.Object = _substanceClassification.Save(sclf.Object as Subst_clf_PK);
                        sclf.ID = (int)(sclf.Object as Subst_clf_PK).subst_clf_PK;

                        Ri_sclf_mn_PK newRiGe = new Ri_sclf_mn_PK()
                        {
                            ref_info_FK = inRefInfo.reference_info_PK,
                            sclf_FK = (sclf.Object as Subst_clf_PK).subst_clf_PK
                        };
                        _ri_sclf_mn_PKOperations.Save(newRiGe);
                    }
                    else if (sclf.Action == ActionType.Edit)
                    {
                        sclf.Object = _substanceClassification.Save(sclf.Object as Subst_clf_PK);
                    }
                }
                else if (sclf.InDB)
                {
                    _substanceClassification.Delete<int>(sclf.ID);
                }

                if (sclf.AssignedObjects.ContainsKey("ReferenceSource"))
                {
                    foreach (var referenceSource in sclf.AssignedObjects["ReferenceSource"])
                    {
                        if (referenceSource.Action == ActionType.New)
                        {
                            Rs_sclf_mn_PK newRsSclf = new Rs_sclf_mn_PK()
                            {
                                rs_FK = (referenceSource.Object as Reference_source_PK).reference_source_PK,
                                sclf_FK = (sclf.Object as Subst_clf_PK).subst_clf_PK
                            };
                            _refSourceSclf.Save(newRsSclf);
                        }
                    }
                }

                if (sclf.AssignedObjects.ContainsKey("Subtype"))
                {
                    foreach (var subtype in sclf.AssignedObjects["Subtype"])
                    {
                        if (subtype.Action == ActionType.New)
                        {
                            Subtype_sclf_mn_PK newSubtypeSclf = new Subtype_sclf_mn_PK()
                            {
                                subtype_FK = (subtype.Object as Subtype_PK).subtype_PK,
                                sclf_FK = (sclf.Object as Subst_clf_PK).subst_clf_PK
                            };
                            _subtypeSclf.Save(newSubtypeSclf);
                        }
                    }
                }


            }
        }

        private void SaveSubstanceCodeToDb(Substance_s_PK inSubstance)
        {
            ISubstance_code_PKOperations _substance_operation = new Substance_code_PKDAL();
            IReference_source_PKOperations _referenceSource = new Reference_source_PKDAL();
            IRs_sc_mn_PKOperations _rs_sc_operation = new Rs_sc_mn_PKDAL();
            ISubstance_substance_code_mn_PKOperations _substance_substance_code_mn_PKOperations = new Substance_substance_code_mn_PKDAL();

            if (!AssignedObjects.ContainsKey("SubstanceCode"))
                return;

            foreach (var substanceCode in AssignedObjects["SubstanceCode"])
            {
                if (substanceCode.Action == ActionType.Delete)
                    DeleteAssociatedObjects(substanceCode);

                if (substanceCode.AssignedObjects.ContainsKey("ReferenceSource"))
                {
                    foreach (var referenceSource in substanceCode.AssignedObjects["ReferenceSource"])
                    {
                        if (referenceSource.Action != ActionType.Delete)
                        {
                            if (referenceSource.Action == ActionType.New)
                            {
                                (referenceSource.Object as Reference_source_PK).reference_source_PK = null;
                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                                referenceSource.ID = (int)(referenceSource.Object as Reference_source_PK).reference_source_PK;

                            }
                            else if (referenceSource.Action == ActionType.Edit)
                            {
                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                            }
                        }
                        else if (referenceSource.InDB)
                        {
                            _referenceSource.Delete<int>(referenceSource.ID);
                        }
                    }
                }

                if (substanceCode.Action != ActionType.Delete)
                {
                    if (substanceCode.Action == ActionType.New)
                    {
                        (substanceCode.Object as Substance_code_PK).substance_code_PK = null;
                        substanceCode.Object = _substance_operation.Save(substanceCode.Object as Substance_code_PK);
                        substanceCode.ID = (int)(substanceCode.Object as Substance_code_PK).substance_code_PK;

                        Substance_substance_code_mn_PK newSubSubCode = new Substance_substance_code_mn_PK()
                        {
                            substance_FK = inSubstance.substance_s_PK,
                            substance_code_FK = (substanceCode.Object as Substance_code_PK).substance_code_PK
                        };
                        _substance_substance_code_mn_PKOperations.Save(newSubSubCode);
                    }
                    else if (substanceCode.Action == ActionType.Edit)
                    {
                        substanceCode.Object = _substance_operation.Save(substanceCode.Object as Substance_code_PK);
                    }
                }
                else if (substanceCode.InDB)
                {
                    _substance_operation.Delete(substanceCode.ID);
                }

                if (substanceCode.AssignedObjects.ContainsKey("ReferenceSource"))
                {
                    foreach (var referenceSource in substanceCode.AssignedObjects["ReferenceSource"])
                    {
                        if (referenceSource.Action == ActionType.New)
                        {
                            Rs_sc_mn_PK newRsSc = new Rs_sc_mn_PK()
                            {
                                rs_FK = (referenceSource.Object as Reference_source_PK).reference_source_PK,
                                sc_FK = (substanceCode.Object as Substance_code_PK).substance_code_PK
                            };
                            _rs_sc_operation.Save(newRsSc);
                        }
                    }
                }

            }
        }

        private void SaveSubstanceNameToDb(Substance_s_PK inSubstance)
        {
            ISubstance_name_PKOperations _substance_operation = new Substance_name_PKDAL();
            IReference_source_PKOperations _referenceSource = new Reference_source_PKDAL();
            IRs_sn_mn_PKOperations _rs_sn_operation = new Rs_sn_mn_PKDAL();
            IOfficial_name_PKOperations _official_name_PKOperations = new Official_name_PKDAL();
            IOn_domain_on_mn_PKOperations _on_domain_PKOperations = new On_domain_on_mn_PKDAL();
            IOn_onj_mn_PKOperations _on_onj_mn_PKOperations = new On_onj_mn_PKDAL();
            ISn_on_mn_PKOperations _sn_on_mn_PKOperations = new Sn_on_mn_PKDAL();
            ISubstance_substance_name_mn_PKOperations _substance_substance_name_mn_PKOperations = new Substance_substance_name_mn_PKDAL();

            if (!AssignedObjects.ContainsKey("SubstanceName"))
                return;

            foreach (var substanceName in AssignedObjects["SubstanceName"])
            {
                if (substanceName.AssignedObjects.ContainsKey("ReferenceSource"))
                {
                    if (substanceName.Action == ActionType.Delete)
                        DeleteAssociatedObjects(substanceName);

                    foreach (var referenceSource in substanceName.AssignedObjects["ReferenceSource"])
                    {
                        if (referenceSource.Action != ActionType.Delete)
                        {
                            if (referenceSource.Action == ActionType.New)
                            {
                                (referenceSource.Object as Reference_source_PK).reference_source_PK = null;
                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                                referenceSource.ID = (int)(referenceSource.Object as Reference_source_PK).reference_source_PK;
                            }
                            else if (referenceSource.Action == ActionType.Edit)
                            {
                                referenceSource.Object = _referenceSource.Save(referenceSource.Object as Reference_source_PK);
                            }
                        }
                        else if (referenceSource.InDB)
                        {
                            DeleteAssociatedObjects(referenceSource);
                            _referenceSource.Delete(referenceSource.ID);
                        }
                    }
                }

                if (substanceName.AssignedObjects.ContainsKey("OfficialName"))
                {
                    foreach (var officialName in substanceName.AssignedObjects["OfficialName"])
                    {
                        if (officialName.Action != ActionType.Delete)
                        {
                            if (officialName.Action == ActionType.New)
                            {
                                (officialName.Object as Official_name_PK).official_name_PK = null;
                                officialName.Object = _official_name_PKOperations.Save(officialName.Object as Official_name_PK);
                                officialName.ID = (int)(officialName.Object as Official_name_PK).official_name_PK;
                            }
                            else if (officialName.Action == ActionType.Edit)
                            {
                                officialName.Object = _official_name_PKOperations.Save(officialName.Object as Official_name_PK);
                            }
                        }
                        else if (officialName.InDB)
                        {
                            DeleteAssociatedObjects(officialName);
                            _official_name_PKOperations.Delete(officialName.ID);
                        }

                        if (officialName.AssignedObjects.ContainsKey("Domain"))
                        {
                            List<On_domain_on_mn_PK> items = _on_domain_PKOperations.GetEntitiesByONPK((officialName.Object as Official_name_PK).official_name_PK);
                            foreach (var domainMN in items) _on_domain_PKOperations.Delete(domainMN.on_domain_on_mn_PK);

                            foreach (var domain in officialName.AssignedObjects["Domain"])
                            {
                                if (domain.Action == ActionType.New)
                                {
                                    On_domain_on_mn_PK newOnDom = new On_domain_on_mn_PK()
                                    {
                                        on_domain_FK = (domain.Object as Ssi__cont_voc_PK).ssi__cont_voc_PK,
                                        on_FK = (officialName.Object as Official_name_PK).official_name_PK
                                    };
                                    _on_domain_PKOperations.Save(newOnDom);
                                }
                            }
                        }

                        if (officialName.AssignedObjects.ContainsKey("Juristiction"))
                        {
                            List<On_onj_mn_PK> items = _on_onj_mn_PKOperations.GetEntitiesByONPK((officialName.Object as Official_name_PK).official_name_PK);
                            foreach (var jurMN in items) _on_onj_mn_PKOperations.Delete(jurMN.on_onj_mn_PK);

                            foreach (var jur in officialName.AssignedObjects["Juristiction"])
                            {
                                if (jur.Action == ActionType.New)
                                {
                                    On_onj_mn_PK newOnJur = new On_onj_mn_PK()
                                    {
                                        onj_FK = (jur.Object as Ssi__cont_voc_PK).ssi__cont_voc_PK,
                                        on_FK = (officialName.Object as Official_name_PK).official_name_PK
                                    };
                                    _on_onj_mn_PKOperations.Save(newOnJur);
                                }
                            }
                        }

                    }
                }

                if (substanceName.Action != ActionType.Delete)
                {
                    if (substanceName.Action == ActionType.New)
                    {
                        (substanceName.Object as Substance_name_PK).substance_name_PK = null;
                        substanceName.Object = _substance_operation.Save(substanceName.Object as Substance_name_PK);
                        substanceName.ID = (int)(substanceName.Object as Substance_name_PK).substance_name_PK;

                        Substance_substance_name_mn_PK newSubSubName = new Substance_substance_name_mn_PK()
                        {
                            substance_FK = inSubstance.substance_s_PK,
                            substance_name_FK = (substanceName.Object as Substance_name_PK).substance_name_PK
                        };
                        _substance_substance_name_mn_PKOperations.Save(newSubSubName);
                    }
                    else if (substanceName.Action == ActionType.Edit)
                    {
                        substanceName.Object = _substance_operation.Save(substanceName.Object as Substance_name_PK);
                    }
                }
                else if (substanceName.InDB)
                {
                    _substance_operation.Delete(substanceName.ID);
                }

                if (substanceName.AssignedObjects.ContainsKey("ReferenceSource"))
                {
                    foreach (var referenceSource in substanceName.AssignedObjects["ReferenceSource"])
                    {
                        if (referenceSource.Action == ActionType.New)
                        {
                            Rs_sn_mn_PK newRsSn = new Rs_sn_mn_PK()
                            {
                                rs_FK = (referenceSource.Object as Reference_source_PK).reference_source_PK,
                                substance_name_FK = (substanceName.Object as Substance_name_PK).substance_name_PK
                            };
                            _rs_sn_operation.Save(newRsSn);
                        }
                    }
                }

                if (substanceName.AssignedObjects.ContainsKey("OfficialName"))
                {
                    foreach (var officialName in substanceName.AssignedObjects["OfficialName"])
                    {
                        if (officialName.Action == ActionType.New)
                        {
                            Sn_on_mn_PK newSnOn = new Sn_on_mn_PK()
                            {
                                official_name_FK = (officialName.Object as Official_name_PK).official_name_PK,
                                substance_name_FK = (substanceName.Object as Substance_name_PK).substance_name_PK
                            };
                            _sn_on_mn_PKOperations.Save(newSnOn);
                        }
                    }
                }


            }
        }


        public void SaveVersionToDb(Substance_s_PK inSubstance)
        {
            IVersion_PKOperations _version_Operations = new Version_PKDAL();
            IVersion_substance_mn_PKOperations _version_substance_mn_PKOperations = new Version_substance_mn_PKDAL();

            if (!AssignedObjects.ContainsKey("Version"))
                return;
            foreach (var ver in AssignedObjects["Version"])
            {
                if (ver.Action != ActionType.Delete)
                {
                    if (ver.Action == ActionType.New)
                    {
                        (ver.Object as Version_PK).version_PK = null;
                        ver.Object = _version_Operations.Save(ver.Object as Version_PK);
                        ver.ID = (int)(ver.Object as Version_PK).version_PK;

                        Version_substance_mn_PK newVerSub = new Version_substance_mn_PK()
                        {
                            substance_FK = inSubstance.substance_s_PK,
                            version_FK = (ver.Object as Version_PK).version_PK
                        };
                        _version_substance_mn_PKOperations.Save(newVerSub);
                    }
                    else if (ver.Action == ActionType.Edit)
                    {
                        ver.Object = _version_Operations.Save(ver.Object as Version_PK);
                    }
                }
                else if (ver.InDB)
                {
                    _version_Operations.Delete(ver.ID);
                }


            }
        }

        public void SaveStructureToDatabase(ObjectContainer inSingOC)
        {
            IIsotope_PKOperations _isotope = new Isotope_PKDAL();
            IStructure_PKOperations _structure = new Structure_PKDAL();
            IIsotope_structure_mn_PKOperations _isotope_Structure_mn = new Isotope_structure_mn_PKDAL();
            ISing_structure_mn_PKOperations _sing_structure_mn_PKOperations = new Sing_structure_mn_PKDAL();

            if (!inSingOC.AssignedObjects.ContainsKey("Structure"))
                return;


            foreach (var structure in inSingOC.AssignedObjects["Structure"])
            {
                if (structure.Action == ActionType.Delete)
                    DeleteAssociatedObjects(structure);

                if (structure.AssignedObjects.ContainsKey("Isotope"))
                {
                    foreach (var isotope in structure.AssignedObjects["Isotope"])
                    {
                        if (isotope.Action != ActionType.Delete)
                        {
                            if (isotope.Action == ActionType.New)
                            {
                                (isotope.Object as Isotope_PK).isotope_PK = null;
                                isotope.Object = _isotope.Save(isotope.Object as Isotope_PK);
                                isotope.ID = (int)(isotope.Object as Isotope_PK).isotope_PK;
                            }
                            else if (isotope.Action == ActionType.Edit)
                            {

                                isotope.Object = _isotope.Save(isotope.Object as Isotope_PK);
                            }
                        }
                        else if (isotope.InDB)
                        {
                            _isotope.Delete<int>(isotope.ID);
                        }
                    }
                }

                if (structure.Action != ActionType.Delete)
                {
                    if (structure.Action == ActionType.New)
                    {
                        (structure.Object as Structure_PK).structure_PK = null;
                        structure.Object = _structure.Save(structure.Object as Structure_PK);
                        structure.ID = (int)(structure.Object as Structure_PK).structure_PK;

                        Sing_structure_mn_PK newSingStruct = new Sing_structure_mn_PK()
                        {
                            sing_FK = inSingOC.ID,
                            structure_FK = (structure.Object as Structure_PK).structure_PK
                        };
                        _sing_structure_mn_PKOperations.Save(newSingStruct);
                    }
                    else if (structure.Action == ActionType.Edit)
                    {
                        structure.Object = _structure.Save(structure.Object as Structure_PK);
                    }
                }
                else if (structure.InDB)
                {
                    _structure.Delete(structure.ID);
                }

                if (structure.AssignedObjects.ContainsKey("Isotope"))
                {
                    foreach (var isotope in structure.AssignedObjects["Isotope"])
                    {
                        if (isotope.Action == ActionType.New)
                        {
                            Isotope_structure_mn_PK newIsotopeStructure = new Isotope_structure_mn_PK()
                            {
                                isotope_FK = (isotope.Object as Isotope_PK).isotope_PK,
                                structure_FK = (structure.Object as Structure_PK).structure_PK
                            };
                            _isotope_Structure_mn.Save(newIsotopeStructure);
                        }
                    }
                }
            }
        }

        public void SaveMoietyToDb(ObjectContainer inNonStoOC)
        {
            IAmount_PKOperations _amount = new Amount_PKDAL();
            IMoiety_PKOperations _moiety = new Moiety_PKDAL();
            INs_moiety_mn_PKOperations _ns_moiety_mn_PKOperations = new Ns_moiety_mn_PKDAL();

            if (!inNonStoOC.AssignedObjects.ContainsKey("Moiety"))
                return;

            foreach (var moiety in inNonStoOC.AssignedObjects["Moiety"])
            {
                if (moiety.Action == ActionType.Delete)
                    DeleteAssociatedObjects(moiety);

                if (moiety.AssignedObjects.ContainsKey("Amount"))
                {
                    foreach (var amount in moiety.AssignedObjects["Amount"])
                    {
                        if (amount.Action != ActionType.Delete)
                        {
                            if (amount.Action == ActionType.New)
                            {
                                (amount.Object as Amount_PK).amount_PK = null;
                                amount.Object = _amount.Save(amount.Object as Amount_PK);
                                amount.ID = (int)(amount.Object as Amount_PK).amount_PK;
                            }
                            else if (amount.Action == ActionType.Edit)
                            {

                                amount.Object = _amount.Save(amount.Object as Amount_PK);
                            }
                            (moiety.Object as Moiety_PK).amount_FK = (amount.Object as Amount_PK).amount_PK;
                        }
                        else if (amount.InDB)
                        {
                            _amount.Delete(amount.ID);
                        }
                    }
                }
                if (moiety.Action != ActionType.Delete)
                {
                    if (moiety.Action == ActionType.New)
                    {
                        (moiety.Object as Moiety_PK).moiety_PK = null;
                        moiety.Object = _moiety.Save(moiety.Object as Moiety_PK);
                        moiety.ID = (int)(moiety.Object as Moiety_PK).moiety_PK;

                        Ns_moiety_mn_PK newNonMoi = new Ns_moiety_mn_PK()
                        {
                            ns_FK = inNonStoOC.ID,
                            moiety_FK = (moiety.Object as Moiety_PK).moiety_PK
                        };
                        _ns_moiety_mn_PKOperations.Save(newNonMoi);
                    }
                    else if (moiety.Action == ActionType.Edit)
                    {
                        moiety.Object = _moiety.Save(moiety.Object as Moiety_PK);
                    }
                }
                else if (moiety.InDB)
                {
                    _moiety.Delete(moiety.ID);
                }


            }
        }

        public void SavePropertyToDb(ObjectContainer inNonStoOC)
        {
            IAmount_PKOperations _amount = new Amount_PKDAL();
            IProperty_PKOperations _property = new Property_PKDAL();
            INs_property_mn_PKOperations _ns_property_mn_PKOperations = new Ns_property_mn_PKDAL();

            if (!inNonStoOC.AssignedObjects.ContainsKey("Property"))
                return;

            foreach (var property in inNonStoOC.AssignedObjects["Property"])
            {
                if (property.Action == ActionType.Delete)
                    DeleteAssociatedObjects(property);

                if (property.AssignedObjects.ContainsKey("Amount"))
                {
                    foreach (var amount in property.AssignedObjects["Amount"])
                    {
                        if (amount.Action != ActionType.Delete)
                        {
                            if (amount.Action == ActionType.New)
                            {
                                (amount.Object as Amount_PK).amount_PK = null;
                                amount.Object = _amount.Save(amount.Object as Amount_PK);
                                amount.ID = (int)(amount.Object as Amount_PK).amount_PK;
                            }
                            else if (amount.Action == ActionType.Edit)
                            {
                                amount.Object = _amount.Save(amount.Object as Amount_PK);
                            }
                            (property.Object as Property_PK).amount_FK = (amount.Object as Amount_PK).amount_PK;
                        }
                        else if (amount.InDB)
                        {
                            _amount.Delete(amount.ID);
                        }
                    }
                }
                if (property.Action != ActionType.Delete)
                {
                    if (property.Action == ActionType.New)
                    {
                        (property.Object as Property_PK).property_PK = null;
                        property.Object = _property.Save(property.Object as Property_PK);
                        property.ID = (int)(property.Object as Property_PK).property_PK;

                        Ns_property_mn_PK newNonPrp = new Ns_property_mn_PK()
                        {
                            ns_FK = inNonStoOC.ID,
                            property_FK = (property.Object as Property_PK).property_PK
                        };
                        _ns_property_mn_PKOperations.Save(newNonPrp);
                    }
                    else if (property.Action == ActionType.Edit)
                    {
                        property.Object = _property.Save(property.Object as Property_PK);
                    }
                }
                else if (property.InDB)
                {
                    _property.Delete(property.ID);
                }
            }
        }
        #endregion

        #region Load from database

        public void LoadSubstanceFromDb(Substance_s_PK inSubstance)
        {
            LoadSNFromDb(inSubstance);
            LoadSCFromDb(inSubstance);
            LoadRIFromDb(inSubstance);
            LoadVERFromDb(inSubstance);
            LoadSingFromDb(inSubstance);
        }

        private void LoadSNFromDb(Substance_s_PK inSubstance)
        {
            ISubstance_name_PKOperations _substance_operation = new Substance_name_PKDAL();
            IReference_source_PKOperations _referenceSource = new Reference_source_PKDAL();
            IRs_sn_mn_PKOperations _rs_sn_operation = new Rs_sn_mn_PKDAL();
            IOfficial_name_PKOperations _official_name_PKOperations = new Official_name_PKDAL();
            IOn_domain_on_mn_PKOperations _on_domain_PKOperations = new On_domain_on_mn_PKDAL();
            IOn_onj_mn_PKOperations _on_onj_mn_PKOperations = new On_onj_mn_PKDAL();
            ISn_on_mn_PKOperations _sn_on_mn_PKOperations = new Sn_on_mn_PKDAL();
            ISubstance_substance_name_mn_PKOperations _substance_substance_name_mn_PKOperations = new Substance_substance_name_mn_PKDAL();
            ISsi__cont_voc_PKOperations _ssi__cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();

            List<Substance_name_PK> itemsSN = _substance_operation.GetSNBySubstancePK(inSubstance.substance_s_PK);

            foreach (var substanceName in itemsSN)
            {
                ObjectContainer ocSN = AddObject((int)substanceName.substance_name_PK, substanceName, "SubstanceName", null);
                ocSN.SetLoadedFromDB();

                List<Reference_source_PK> itemsRS = _referenceSource.GetRSBySNPK(substanceName.substance_name_PK);

                foreach (var referenceSource in itemsRS)
                {
                    ObjectContainer ocRS = AddObject((int)referenceSource.reference_source_PK, referenceSource, "ReferenceSource", ocSN);
                    ocRS.SetLoadedFromDB();
                }

                List<Official_name_PK> itemsON = _official_name_PKOperations.GetONBySNPK(substanceName.substance_name_PK);

                foreach (var officialName in itemsON)
                {
                    ObjectContainer ocON = AddObject((int)officialName.official_name_PK, officialName, "OfficialName", ocSN);
                    ocON.SetLoadedFromDB();

                    List<Ssi__cont_voc_PK> itemsDomains = _ssi__cont_voc_PKOperations.GetDomainByONPK(officialName.official_name_PK);

                    foreach (var domain in itemsDomains)
                    {
                        ObjectContainer ocDomain = AddObject((int)domain.ssi__cont_voc_PK, domain, "Domain", ocON);
                        ocDomain.SetLoadedFromDB();
                    }

                    List<Ssi__cont_voc_PK> itemsJur = _ssi__cont_voc_PKOperations.GetJurByONPK(officialName.official_name_PK);

                    foreach (var juristicton in itemsJur)
                    {
                        ObjectContainer ocJuristiction = AddObject((int)juristicton.ssi__cont_voc_PK, juristicton, "Juristiction", ocON);
                        ocJuristiction.SetLoadedFromDB();
                    }
                }
            }
        }

        private void LoadSCFromDb(Substance_s_PK inSubstance)
        {
            ISubstance_code_PKOperations _substance_operation = new Substance_code_PKDAL();
            IReference_source_PKOperations _referenceSource = new Reference_source_PKDAL();
            IRs_sc_mn_PKOperations _rs_sc_operation = new Rs_sc_mn_PKDAL();
            ISubstance_substance_code_mn_PKOperations _substance_substance_code_mn_PKOperations = new Substance_substance_code_mn_PKDAL();

            List<Substance_code_PK> itemsSC = _substance_operation.GetSCBySubstancePK(inSubstance.substance_s_PK);

            foreach (var substanceCode in itemsSC)
            {

                ObjectContainer ocSC = AddObject((int)substanceCode.substance_code_PK, substanceCode, "SubstanceCode", null);
                ocSC.SetLoadedFromDB();

                List<Reference_source_PK> itemsRS = _referenceSource.GetRSBySCPK(substanceCode.substance_code_PK);

                foreach (var referenceSource in itemsRS)
                {
                    ObjectContainer ocRS = AddObject((int)referenceSource.reference_source_PK, referenceSource, "ReferenceSource", ocSC);
                    ocRS.SetLoadedFromDB();
                }

            }
        }

        private void LoadRIFromDb(Substance_s_PK inSubstance)
        {
            IReference_info_PKOperations _reference_info_PKOperations = new Reference_info_PKDAL();
            IReference_source_PKOperations _referenceSource = new Reference_source_PKDAL();
            IGene_element_PKOperations _geneElement = new Gene_element_PKDAL();
            ITarget_PKOperations _targetOperation = new Target_PKDAL();
            ISubst_clf_PKOperations _substanceClassification = new Subst_clf_PKDAL();
            ISubtype_PKOperations _subtype = new Subtype_PKDAL();
            IGene_PKOperations _gene = new Gene_PKDAL();
            ISubstance_relationship_PKOperations _substanceRelationship = new Substance_relationship_PKDAL();
            IAmount_PKOperations _amountOperations = new Amount_PKDAL();


            Reference_info_PK refInfo = _reference_info_PKOperations.GetEntity(inSubstance.ref_info_FK);
            ObjectContainer ocRI = AddObject((int)refInfo.reference_info_PK, refInfo, "ReferenceInformation", null);
            ocRI.SetLoadedFromDB();

            List<Gene_element_PK> itemsGE = _geneElement.GetGEByRIPK(refInfo.reference_info_PK);

            foreach (var geneElement in itemsGE)
            {
                ObjectContainer ocGE = AddObject((int)geneElement.gene_element_PK, geneElement, "GeneElement", ocRI);
                ocGE.SetLoadedFromDB();

                List<Reference_source_PK> itemsRS = _referenceSource.GetRSByGEPK(geneElement.gene_element_PK);

                foreach (var referenceSource in itemsRS)
                {
                    ObjectContainer ocRS = AddObject((int)referenceSource.reference_source_PK, referenceSource, "ReferenceSource", ocGE);
                    ocRS.SetLoadedFromDB();
                }
            }

            List<Target_PK> itemsTrg = _targetOperation.GetTargetByRIPK(refInfo.reference_info_PK);

            foreach (var target in itemsTrg)
            {
                ObjectContainer ocTrg = AddObject((int)target.target_PK, target, "Target", ocRI);
                ocTrg.SetLoadedFromDB();

                List<Reference_source_PK> itemsRS = _referenceSource.GetRSByTRG(target.target_PK);

                foreach (var referenceSource in itemsRS)
                {
                    ObjectContainer ocRS = AddObject((int)referenceSource.reference_source_PK, referenceSource, "ReferenceSource", ocTrg);
                    ocRS.SetLoadedFromDB();
                }
            }

            List<Subst_clf_PK> itemsSCLF = _substanceClassification.GetSCLFByRIPK(refInfo.reference_info_PK);

            foreach (var sclf in itemsSCLF)
            {
                ObjectContainer ocSCLF = AddObject((int)sclf.subst_clf_PK, sclf, "SubstanceClassification", ocRI);
                ocSCLF.SetLoadedFromDB();

                List<Reference_source_PK> itemsRS = _referenceSource.GetRSBySCLFPK(sclf.subst_clf_PK);

                foreach (var referenceSource in itemsRS)
                {
                    ObjectContainer ocRS = AddObject((int)referenceSource.reference_source_PK, referenceSource, "ReferenceSource", ocSCLF);
                    ocRS.SetLoadedFromDB();
                }

                List<Subtype_PK> itemsSubtype = _subtype.GetSubtypeBySCLFPK(sclf.subst_clf_PK);

                foreach (var subtype in itemsSubtype)
                {
                    ObjectContainer ocSubtype = AddObject((int)subtype.subtype_PK, subtype, "Subtype", ocSCLF);
                    ocSubtype.SetLoadedFromDB();
                }
            }

            List<Gene_PK> itemsGene = _gene.GetGeneByRIPK(refInfo.reference_info_PK);

            foreach (var gene in itemsGene)
            {
                ObjectContainer ocGene = AddObject((int)gene.gene_PK, gene, "Gene", ocRI);
                ocGene.SetLoadedFromDB();

                List<Reference_source_PK> itemsRS = _referenceSource.GetRSByGenePK(gene.gene_PK);

                foreach (var referenceSource in itemsRS)
                {
                    ObjectContainer ocRS = AddObject((int)referenceSource.reference_source_PK, referenceSource, "ReferenceSource", ocGene);
                    ocRS.SetLoadedFromDB();
                }
            }

            List<Substance_relationship_PK> itemsREL = _substanceRelationship.GetRELByRIPK(refInfo.reference_info_PK);

            foreach (var rel in itemsREL)
            {
                ObjectContainer ocREL = AddObject((int)rel.substance_relationship_PK, rel, "SubstanceRelationship", ocRI);
                ocREL.SetLoadedFromDB();

                List<Reference_source_PK> itemsRS = _referenceSource.GetRSByRELPK(rel.substance_relationship_PK);

                foreach (var referenceSource in itemsRS)
                {
                    ObjectContainer ocRS = AddObject((int)referenceSource.reference_source_PK, referenceSource, "ReferenceSource", ocREL);
                    ocRS.SetLoadedFromDB();
                }

                Amount_PK amount = _amountOperations.GetEntity(rel.amount_FK);

                ObjectContainer ocAmount = AddObject((int)amount.amount_PK, amount, "Amount", ocREL);
                ocAmount.SetLoadedFromDB();

            }

        }

        private void LoadVERFromDb(Substance_s_PK inSubstance)
        {
            IVersion_PKOperations _version_Operations = new Version_PKDAL();
            IVersion_substance_mn_PKOperations _version_substance_mn_PKOperations = new Version_substance_mn_PKDAL();

            List<Version_PK> itemsVER = _version_Operations.GetVERBySubstancePK(inSubstance.substance_s_PK);

            foreach (var version in itemsVER)
            {
                ObjectContainer ocVER = AddObject((int)version.version_PK, version, "Version", null);
                ocVER.SetLoadedFromDB();
            }
        }

        private void LoadSingFromDb(Substance_s_PK inSubstance)
        {
            IIsotope_PKOperations _isotope = new Isotope_PKDAL();
            IStructure_PKOperations _structure = new Structure_PKDAL();
            ISing_PKOperations _sing_PKOperations = new Sing_PKDAL();
            IChemical_PKOperations _chemical_PKOperations = new Chemical_PKDAL();
            INon_stoichiometric_PKOperations _non_stoichiometric_PKOperations = new Non_stoichiometric_PKDAL();

            if (inSubstance.sing_FK == null) return;

            Sing_PK sing = _sing_PKOperations.GetEntity(inSubstance.sing_FK);
            ObjectContainer ocSing = AddObject((int)sing.sing_PK, sing, "Sing", null);
            ocSing.SetLoadedFromDB();

            List<Structure_PK> itemsStruct = _structure.GetStructBySingPK(sing.sing_PK);

            foreach (var structure in itemsStruct)
            {
                ObjectContainer ocStruct = AddObject((int)structure.structure_PK, structure, "Structure", ocSing);
                ocStruct.SetLoadedFromDB();

                List<Isotope_PK> itemsISO = _isotope.GetISOByStructPK(structure.structure_PK);

                foreach (var isotope in itemsISO)
                {
                    ObjectContainer ocISO = AddObject((int)isotope.isotope_PK, isotope, "Isotope", ocStruct);
                    ocISO.SetLoadedFromDB();
                }

            }

            if (sing.chemical_FK != null)
            {

                Chemical_PK chemical = _chemical_PKOperations.GetEntity(sing.chemical_FK);
                ObjectContainer ocChemical = AddObject((int)chemical.chemical_PK, chemical, "Chemical", ocSing);
                ocChemical.SetLoadedFromDB();

                if (chemical.non_stoichio_FK != null)
                {
                    IAmount_PKOperations _amount = new Amount_PKDAL();
                    IMoiety_PKOperations _moiety = new Moiety_PKDAL();
                    IProperty_PKOperations _property = new Property_PKDAL();

                    Non_stoichiometric_PK nonSto = _non_stoichiometric_PKOperations.GetEntity(chemical.non_stoichio_FK);
                    ObjectContainer ocNonSto = AddObject((int)nonSto.non_stoichiometric_PK, nonSto, "NonStoichiometric", ocChemical);
                    ocNonSto.SetLoadedFromDB();

                    List<Moiety_PK> itemsMoiety = _moiety.GetMoietyByNonStoPK(nonSto.non_stoichiometric_PK);

                    foreach (var moiety in itemsMoiety)
                    {
                        ObjectContainer ocMoiety = AddObject((int)moiety.moiety_PK, moiety, "Moiety", ocNonSto);
                        ocMoiety.SetLoadedFromDB();

                        Amount_PK amount = _amount.GetEntity(moiety.amount_FK);
                        ObjectContainer ocAmount = AddObject((int)amount.amount_PK, amount, "Amount", ocMoiety);
                        ocAmount.SetLoadedFromDB();
                    }

                    List<Property_PK> itemsProperty = _property.GetPropertyByNonStoPK(nonSto.non_stoichiometric_PK);

                    foreach (var property in itemsProperty)
                    {
                        ObjectContainer ocProperty = AddObject((int)property.property_PK, property, "Property", ocNonSto);
                        ocProperty.SetLoadedFromDB();

                        Amount_PK amount = _amount.GetEntity(property.amount_FK);
                        ObjectContainer ocAmount = AddObject((int)amount.amount_PK, amount, "Amount", ocProperty);
                        ocAmount.SetLoadedFromDB();
                    }
                }
            }
        }

        public void LoadApprovedSubstanceFromDb(Approved_substance_PK inApprovedSubstance)
        {
            LoadSubTranFromDb(inApprovedSubstance);
            LoadSubAlsFromDb(inApprovedSubstance);
            LoadIntCodFromDb(inApprovedSubstance);
            LoadPrevCodeFromDb(inApprovedSubstance);
            LoadSubAtcFromDb(inApprovedSubstance);

        }

        private void LoadSubTranFromDb(Approved_substance_PK inApprovedSubstance)
        {
            ISubstance_translations_PKOperations _subTranslationOperations = new Substance_translations_PKDAL();

            List<Substance_translations_PK> subTranslations = _subTranslationOperations.GetSubTranslationByAS(inApprovedSubstance.approved_substance_PK);

            foreach (var item in subTranslations)
            {
                ObjectContainer ocSubTran = AddObject((int)item.substance_translations_PK, item, "SubstanceTranslation", null);
                ocSubTran.SetLoadedFromDB();
            }
        }

        private void LoadSubAlsFromDb(Approved_substance_PK inApprovedSubstance)
        {
            ISubstance_alias_PKOperations _substanceAliasOperations = new Substance_alias_PKDAL();
            ISubstance_alias_translation_PKOperations _subAliasTranslationOperations = new Substance_alias_translation_PKDAL();

            List<Substance_alias_PK> subAliases = _substanceAliasOperations.GetSubAlsByAs(inApprovedSubstance.approved_substance_PK);

            foreach (var item in subAliases)
            {
                ObjectContainer ocSubAls = AddObject((int)item.substance_alias_PK, item, "SubstanceAlias", null);
                ocSubAls.SetLoadedFromDB();

                List<Substance_alias_translation_PK> subAliasTranslations = _subAliasTranslationOperations.GetSubAliasTranBySubAlias(item.substance_alias_PK);

                foreach (var subAliasTran in subAliasTranslations)
                {
                    ObjectContainer ocSubAliasTrans = AddObject((int)subAliasTran.substance_alias_translation_PK, subAliasTran, "AliasTranslation", ocSubAls);
                    ocSubAliasTrans.SetLoadedFromDB();
                }
            }
        }

        private void LoadIntCodFromDb(Approved_substance_PK inApprovedSubstance)
        {
            IInternational_code_PKOperations _internationalCodeOperations = new International_code_PKDAL();

            List<International_code_PK> intCodes = _internationalCodeOperations.GetIntCodesByAs(inApprovedSubstance.approved_substance_PK);

            foreach (var item in intCodes)
            {
                ObjectContainer ocIntCode = AddObject((int)item.international_code_PK, item, "InternationalCode", null);
                ocIntCode.SetLoadedFromDB();
            }
        }

        private void LoadPrevCodeFromDb(Approved_substance_PK inApprovedSubstance)
        {
            IAs_previous_ev_code_PKOperations _previousEvcodeOperations = new As_previous_ev_code_PKDAL();

            List<As_previous_ev_code_PK> previousEvcodeOperations = _previousEvcodeOperations.GetPrevEvcodeByAs(inApprovedSubstance.approved_substance_PK);

            foreach (var item in previousEvcodeOperations)
            {
                ObjectContainer ocPrevEvcode = AddObject((int)item.as_previous_ev_code_PK, item, "PreviousEvcode", null);
                ocPrevEvcode.SetLoadedFromDB();
            }
        }

        private void LoadSubAtcFromDb(Approved_substance_PK inApprovedSubstance)
        {
            ISubstance_attachment_PKOperations _attachmentOperations = new Substance_attachment_PKDAL();

            List<Substance_attachment_PK> substanceAttachment = _attachmentOperations.GetSubAttByAs(inApprovedSubstance.approved_substance_PK);

            foreach (var item in substanceAttachment)
            {
                ObjectContainer ocSubAtt = AddObject((int)item.substance_attachment_PK, item, "SubstanceAttachment", null);
                ocSubAtt.SetLoadedFromDB();
            }
        }

        #endregion

    }
}
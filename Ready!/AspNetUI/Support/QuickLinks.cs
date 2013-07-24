using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using Ready.Model;

namespace AspNetUI.Support
{
    public class QuickLinks
    {
        #region Declarations

        private static class NavigateUrl
        {
            private const string Xevprm = "~/Views/XevprmView/List.aspx";
            private const string AuthorisedProduct = "~/Views/AuthorisedProductView/List.aspx";
            private const string Product = "~/Views/ProductView/List.aspx";
            private const string PharmaceuticalProduct = "~/Views/PharmaceuticalProductView/List.aspx";
            private const string SubmissionUnit = "~/Views/SubmissionUnitView/List.aspx";
            private const string Project = "~/Views/ProjectView/List.aspx";
            private const string Activity = "~/Views/ActivityView/List.aspx";
            private const string Task = "~/Views/TaskView/List.aspx";
            private const string TimeUnit = "~/Views/TimeUnitView/List.aspx";
            private const string Document = "~/Views/DocumentViewAll/List.aspx";
            private const string Alerter = "~/Views/AlerterView/List.aspx";

            private static readonly Dictionary<QuickLinkType, string> QuickLinkUrl = new Dictionary<QuickLinkType, string>
            {
                {QuickLinkType.Xevprm, Xevprm},
                {QuickLinkType.AuthorisedProduct, AuthorisedProduct},
                {QuickLinkType.Product, Product},
                {QuickLinkType.PharmaceuticalProduct, PharmaceuticalProduct},
                {QuickLinkType.SubmissionUnit, SubmissionUnit},
                {QuickLinkType.Project, Project},
                {QuickLinkType.Activity, Activity},
                {QuickLinkType.Task, Task},
                {QuickLinkType.TimeUnit, TimeUnit},
                {QuickLinkType.Document,Document},   
                {QuickLinkType.Alerter,Alerter}
            };

            public static QuickLinkType GetQuickLinkType(string localPath)
            {
                return Enum.GetValues(typeof (QuickLinkType)).Cast<QuickLinkType>().FirstOrDefault(quickLinkType => QuickLinkUrl.ContainsKey(quickLinkType) && localPath.Contains(QuickLinkUrl[quickLinkType]));
            }

            public static string GetUrl(QuickLinkType quickLinktype, string idLay, int? idSearch, bool isSearch)
            {
                string url = QuickLinkUrl[quickLinktype];

                var query = new StringBuilder();

                if (quickLinktype.In(QuickLinkType.Activity, QuickLinkType.TimeUnit) && idLay.In("my","alert"))
                {
                    query.AppendFormat("&EntityContext={0}My", quickLinktype);
                }
                else
                {
                    query.AppendFormat("&EntityContext={0}", quickLinktype);    
                }

                if (isSearch)
                {
                    query.Append("&Action=Search");
                }
                if (idSearch.HasValue)
                {
                    query.Append("&idSearch=" + idSearch);
                }
                if (!string.IsNullOrWhiteSpace(idLay))
                {
                    query.Append("&idLay=" + idLay);
                }

                if (quickLinktype.In(QuickLinkType.Alerter) && idLay.In("my"))
                {
                    query.Append("&ShowAll=False");
                }
                else if (quickLinktype.In(QuickLinkType.Alerter) && idLay.In("default"))
                {
                    query.Append("&ShowAll=True");
                }
                
                if (query.Length > 0)
                {
                    query.Insert(0, "?");
                    query.Replace("?&", "?", 0, 2);
                }

                return url + query;
            }
        }

        private enum QuickLinkType
        {
            NULL, Xevprm, AuthorisedProduct, Product, PharmaceuticalProduct, SubmissionUnit, Project, Activity, Task, TimeUnit, Document, Alerter
        }

        private class QuickLink
        {
            public QuickLinkType QuickLinkType { get; set; }
            public string NavigateUrl { get; set; }
            public string SearchNavigateUrl { get; set; }
            public int? SearchId { get; set; }
            public string Name { get; set; }
            public string ApplicationType { get; set; }
            public bool Selected { get; set; }
        }

        private static readonly Dictionary<QuickLinkType, string> QuickLinkName = new Dictionary<QuickLinkType, string>
        {
            {QuickLinkType.Xevprm, "xEVPRM"}, 
            {QuickLinkType.AuthorisedProduct, "Authorised products"},
            {QuickLinkType.Product, "Products"},
            {QuickLinkType.PharmaceuticalProduct, "Pharmaceutical products"},
            {QuickLinkType.SubmissionUnit, "Submission units"},
            {QuickLinkType.Project, "Projects"},
            {QuickLinkType.Activity, "Activities"},
            {QuickLinkType.Task, "Tasks"},
            {QuickLinkType.TimeUnit, "Time units"},
            {QuickLinkType.Document, "Documents"},
            {QuickLinkType.Alerter, "Alerts"}
        };

        private static List<QuickLink> _quickLinks;

        private static IQuickLinkOperations _quickLinkOperations;

        #endregion

        #region Methods

        /// <summary>
        /// Generate quick links
        /// </summary>
        /// <param name="userPk">Logged in user PK</param>
        /// <param name="selectedQuickLink">Tuple(localPath, idSearch, idLay)</param>
        /// <returns></returns>
        public static Control GenerateQuickLinks(int? userPk = null, Tuple<string, int?, string> selectedQuickLink = null)
        {
            _quickLinkOperations = new QuickLinkDAL();

            _quickLinks = new List<QuickLink>();

            Tuple<QuickLinkType, int?, string> selectQuickLink = null;

            if (selectedQuickLink != null)
            {
                selectQuickLink = new Tuple<QuickLinkType, int?, string>(NavigateUrl.GetQuickLinkType(selectedQuickLink.Item1), selectedQuickLink.Item2, selectedQuickLink.Item3);
            }

            //Get quick links from database
            var quickLinksDs = userPk.HasValue ? _quickLinkOperations.GetEntitiesByUserOrPublic(userPk.Value) : _quickLinkOperations.GetPublicEntities();
            var quickLinksDt = quickLinksDs.Tables.Count > 0 && quickLinksDs.Tables[0].Rows.Count > 0 ? quickLinksDs.Tables[0] : null;

            //Add quick links from database
            if (quickLinksDt != null && quickLinksDt.Columns.Contains("SearchType") && quickLinksDt.Columns.Contains("SearchId") && quickLinksDt.Columns.Contains("SearchName"))
            {
                foreach (DataRow row in quickLinksDt.Rows)
                {
                    var quickLinkDb = new QuickLink();

                    QuickLinkType quickLinkType;
                    if (!Enum.TryParse(Convert.ToString(row["SearchType"]), true, out quickLinkType)) continue;

                    int searchId;
                    if (!int.TryParse(Convert.ToString(row["SearchId"]), out searchId)) continue;

                    quickLinkDb.QuickLinkType = quickLinkType;
                    quickLinkDb.SearchId = searchId;
                    quickLinkDb.NavigateUrl = NavigateUrl.GetUrl(quickLinkType, null, searchId, false);
                    quickLinkDb.SearchNavigateUrl = NavigateUrl.GetUrl(quickLinkType, null, searchId, true);
                    quickLinkDb.Name = Convert.ToString(row["SearchName"]);

                    if (selectQuickLink != null &&
                        quickLinkDb.QuickLinkType == selectQuickLink.Item1 && quickLinkDb.SearchId == selectQuickLink.Item2 &&
                        string.IsNullOrWhiteSpace(selectQuickLink.Item3))
                    {
                        quickLinkDb.Selected = true;
                    }

                    else
                    {
                        quickLinkDb.Selected = false;
                    }

                    _quickLinks.Add(quickLinkDb);
                }
            }

            //Add 'All', 'My' and 'Alert' quick links
            foreach (QuickLinkType quickLinkType in Enum.GetValues(typeof(QuickLinkType)))
            {
                if (quickLinkType == QuickLinkType.NULL) continue;

                var quickLinkAll = new QuickLink
                {
                    QuickLinkType = quickLinkType,
                    SearchId = null,
                    NavigateUrl = NavigateUrl.GetUrl(quickLinkType, "default", null, false),
                    SearchNavigateUrl = null,
                    Name = "All",
                    ApplicationType = "All"
                };

                if (selectQuickLink != null &&
                        quickLinkAll.QuickLinkType == selectQuickLink.Item1 && quickLinkAll.SearchId == selectQuickLink.Item2 &&
                        selectQuickLink.Item3 == "default")
                {
                    quickLinkAll.Selected = true;
                }

                else
                {
                    quickLinkAll.Selected = false;
                }

                _quickLinks.Add(quickLinkAll);

                if (!new[] { QuickLinkType.Xevprm, QuickLinkType.SubmissionUnit }.Contains(quickLinkType))
                {
                    var quickLinkMy = new QuickLink
                    {
                        QuickLinkType = quickLinkType,
                        SearchId = null,
                        NavigateUrl = NavigateUrl.GetUrl(quickLinkType, "my", null, false),
                        SearchNavigateUrl = null,
                        Name = "My " + QuickLinkName[quickLinkType].ToLower(),
                        ApplicationType = "My"
                    };

                    if (selectQuickLink != null &&
                        quickLinkMy.QuickLinkType == selectQuickLink.Item1 && quickLinkMy.SearchId == selectQuickLink.Item2 &&
                        selectQuickLink.Item3 == "my")
                    {
                        quickLinkMy.Selected = true;
                    }

                    else
                    {
                        quickLinkMy.Selected = false;
                    }

                    _quickLinks.Add(quickLinkMy);
                }

                if (new[] { QuickLinkType.Project, QuickLinkType.Activity, QuickLinkType.Task }.Contains(quickLinkType))
                {
                    var quickLinkAlert = new QuickLink
                    {
                        QuickLinkType = quickLinkType,
                        SearchId = null,
                        NavigateUrl = NavigateUrl.GetUrl(quickLinkType, "alert", null, false),
                        SearchNavigateUrl = null,
                        Name = "My alerts",
                        ApplicationType = "Alert"
                    };

                    if (selectQuickLink != null &&
                        quickLinkAlert.QuickLinkType == selectQuickLink.Item1 && quickLinkAlert.SearchId == selectQuickLink.Item2 &&
                        selectQuickLink.Item3 == "alert")
                    {
                        quickLinkAlert.Selected = true;
                    }

                    else
                    {
                        quickLinkAlert.Selected = false;
                    }

                    _quickLinks.Add(quickLinkAlert);
                }
            }

            _quickLinks.Sort(QuickLinkCompare);

            return GenerateQuickLinksControl(_quickLinks);
        }

        private static int QuickLinkCompare(QuickLink q1, QuickLink q2)
        {
            if (q1 == null || q2 == null) return 0;

            var typeCompareResult = q1.QuickLinkType.CompareTo(q2.QuickLinkType);

            if (typeCompareResult != 0) return typeCompareResult;

            if (q1.ApplicationType == "All" && q2.ApplicationType != "All") return -1;
            if (q2.ApplicationType == "All" && q1.ApplicationType != "All") return 1;

            if (q1.ApplicationType == "My" && q2.ApplicationType != "My") return -1;
            if (q2.ApplicationType == "My" && q1.ApplicationType != "My") return 1;

            if (q1.ApplicationType == "Alert" && q2.ApplicationType != "Alert") return -1;
            if (q2.ApplicationType == "Alert" && q1.ApplicationType != "Alert") return 1;

            return q1.Name.CompareTo(q2.Name);
        }

        #region Generate quick links control

        private static Control GenerateQuickLinksControl(List<QuickLink> quickLinks)
        {
            if (quickLinks == null || quickLinks.Count == 0) return null;

            var control = new Control();

            var ulStartLiteralControl = new LiteralControl("<ul>");
            control.Controls.Add(ulStartLiteralControl);

            foreach (QuickLinkType quickLinkType in Enum.GetValues(typeof(QuickLinkType)))
            {
                AddQuickLinkGroupListItem(ref control, quickLinks.Where(item => item.QuickLinkType == quickLinkType).ToList(), quickLinkType == quickLinks.Last().QuickLinkType);
            }

            var ulEndLiteralControl = new LiteralControl("</ul>");
            control.Controls.Add(ulEndLiteralControl);

            return control;
        }

        private static void AddQuickLinkGroupListItem(ref Control control, List<QuickLink> quickLinks, bool isLast)
        {
            if (quickLinks == null || quickLinks.Count == 0) return;

            var liStartLiteralControl = new LiteralControl(isLast ? "<li class=\"last\"><span></span>" : "<li><span></span>");
            control.Controls.Add(liStartLiteralControl);

            var qlGroupLiteralControl = new LiteralControl(string.Format("<a href=\"\" class=\"ql-groupname\">{0}</a>", QuickLinkName[quickLinks[0].QuickLinkType]));
            control.Controls.Add(qlGroupLiteralControl);

            var ulStartLiteralControl = new LiteralControl("<ul>");
            control.Controls.Add(ulStartLiteralControl);

            foreach (var quickLink in quickLinks)
            {
                AddQuickLinkListItem(ref control, quickLink, quickLink == quickLinks.Last());
            }

            var liEndLiteralControl = new LiteralControl("</ul></li>");
            control.Controls.Add(liEndLiteralControl);

        }

        private static void AddQuickLinkListItem(ref Control control, QuickLink quickLink, bool isLast)
        {
            if (quickLink == null) return;

            var liStartLiteralControl = new LiteralControl(isLast ? "<li class=\"last\">" : "<li>");
            control.Controls.Add(liStartLiteralControl);

            var quickLinkName = !string.IsNullOrWhiteSpace(quickLink.Name) ? quickLink.Name : "N/A";
            quickLinkName = quickLinkName.Length > 23 ? quickLinkName.Substring(0, 20) + "..." : quickLinkName;

            var quickLinkHyperLink = new HyperLink
            {
                Text = quickLinkName,
                ToolTip = quickLink.Name,
                NavigateUrl = quickLink.NavigateUrl
            };

            if (quickLink.Selected)
            {
                quickLinkHyperLink.Attributes.Add("class", "selected-quick-link");
            }

            control.Controls.Add(quickLinkHyperLink);

            if (!string.IsNullOrWhiteSpace(quickLink.SearchNavigateUrl))
            {
                var quickLinkSearchHyperLink = new HyperLink { NavigateUrl = quickLink.SearchNavigateUrl };
                quickLinkSearchHyperLink.Attributes.Add("class", "ql-icon saved-search");

                control.Controls.Add(quickLinkSearchHyperLink);
            }

            var liEndLiteralControl = new LiteralControl("</li>");
            control.Controls.Add(liEndLiteralControl);
        }

        #endregion

        #endregion
    }
}
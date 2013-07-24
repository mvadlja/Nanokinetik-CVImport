using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
namespace AspNetUI.TimeUnitGrid
{
  public class Pager {       

        string url = LinkHelper.getQueryStringArray(new string[] { "page", "pageSize" });
            
        public String RenderPager(HttpRequest Request, int numberOfRecords){
            int page = !String.IsNullOrWhiteSpace(Request.QueryString["page"]) ? Convert.ToInt32(Request.QueryString["page"]) : 1;
            int pageSize = (HttpContext.Current.Session["gridViewItemsPerPage"]) != null ? (int)(HttpContext.Current.Session["gridViewItemsPerPage"]) : 15;   
    
            bool showPager          = true;
            bool showPagerInfo      = true;
            bool showPages          = true;
    
            if(page < 1) { page = 1; }        
            if(numberOfRecords == 0) { page = 0; }            
    
            int from                = (page*pageSize) - pageSize;
            int to                  = page*pageSize;

            int numberOfPages       = (numberOfRecords + pageSize - 1) / pageSize;
            
            int firstPage           = 1;
            int previousPage        = page - 1;
            if(previousPage < firstPage) { previousPage = firstPage; }
            
            int lastPage            = numberOfPages;
            int nextPage            = page + 1;
            if(nextPage > lastPage) { nextPage = lastPage; }
            
            int minMaxRange         = 3;
            int centerRange         = 2;
    
            if(page == lastPage) { to = numberOfRecords; }
            StringBuilder sb = new StringBuilder();
            if(showPager == true) 
            {
                sb.AppendLine("<div id=\"pager\">");
                    if(showPagerInfo == true) 
                    {
                        sb.AppendLine("<div id=\"pager_info\">Page "+ page +" of "+numberOfPages+" ("+numberOfRecords+" items)</div>");
                    }
                    if(showPages == true) 
                    {
                        sb.AppendLine("<div id=\"pages\">");
                    
                            if(page == firstPage || numberOfPages == 0) 
                            {
                                sb.AppendLine("<span class=\"previous inactive\"></span>"); 
                            }
                            else
                            {
                                sb.AppendLine("<a href=\"javascript:RefreshGrid('" + url + "page=" + previousPage + "&pageSize=" + pageSize + "');\" class=\"previous\"></a>"); 
                            }

                            for(int i = 1; i <= numberOfPages; i++) 
                            {

                                if (i <= minMaxRange || i > numberOfPages - minMaxRange || (i > page - centerRange && i < page + centerRange + 1) || (page <= minMaxRange + centerRange && i <= minMaxRange + centerRange*2) || (page > numberOfPages - minMaxRange - centerRange && i>numberOfPages-minMaxRange-centerRange*2))
                                {    
                                    if(i == page) 
                                    {
                                        sb.AppendLine("<a class=\"current\">["+i+"]</a>"); 
                                    }
                                    else if (i == minMaxRange && page > minMaxRange + centerRange && numberOfPages > 2 * (minMaxRange + centerRange)) 
                                    {
                                        sb.AppendLine("<a href=\"javascript:RefreshGrid('" + url + "page=" + i + "&pageSize=" + pageSize + "');\" class=\"margin-right-5\">" + i + "</a>");
                                        sb.AppendLine("<span class=\"margin-right-5\">...</span>");
                                    } 
                                    else if(i == numberOfPages + 1 - minMaxRange && page < numberOfPages - minMaxRange - centerRange && numberOfPages>2*(minMaxRange+centerRange)) 
                                    {
                                        sb.AppendLine("<span class=\"margin-left-5\">...</span>");
                                        sb.AppendLine("<a href=\"javascript:RefreshGrid('" + url + "page=" + i + "&pageSize=" + pageSize + "');\" class=\"margin-left-5\">" + i + "</a>");
                                    } 
                                    else 
                                    {
                                        sb.AppendLine("<a href=\"javascript:RefreshGrid('" + url + "page=" + i + "&pageSize=" + pageSize + "');\">" + i + "</a>");
                                    }
                                    
                                }
                                  
                            }

                            if(page == lastPage || numberOfPages == 0) 
                            {
                                sb.AppendLine("<span class=\"next inactive\"></span>"); 
                            }
                            else
                            {
                                sb.AppendLine("<a href=\"javascript:RefreshGrid('" + url + "page=" + nextPage + "&pageSize=" + pageSize + "');\" class=\"next\"></a>");
                            }

                        sb.AppendLine("</div>");
                    }
                    sb.AppendLine("</div>");
           
            }
            return sb.ToString();
        }
    }
}
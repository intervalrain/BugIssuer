using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BugIssuer.Web.Extensions;

public static class StatusModeling
{
    public static ViewDataDictionary UpdateStatus(ViewDataDictionary viewData, string sortOrder, string filterStatus)
    {
        viewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
        viewData["CategorySortParm"] = sortOrder == "Category" ? "category_desc" : "Category";
        viewData["TitleSortParm"] = sortOrder == "Title" ? "title_desc" : "Title";
        viewData["AuthorSortParm"] = sortOrder == "Author" ? "author_desc" : "Author";
        viewData["DateTimeSortParm"] = sortOrder == "DateTime" ? "datetime_desc" : "DateTime";
        viewData["LastUpdateSortParm"] = sortOrder == "LastUpdate" ? "lastupdate_desc" : "LastUpdate";
        viewData["CommentsSortParm"] = sortOrder == "Comments" ? "comments_desc" : "Comments";
        viewData["AssigneeSortParm"] = sortOrder == "Assignee" ? "assignee_desc" : "Assignee";
        viewData["UrgencySortParm"] = sortOrder == "Urgency" ? "urgency_desc" : "Urgency";
        viewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";

        viewData["CurrentFilter"] = filterStatus;
        viewData["CurrentSort"] = sortOrder;

        return viewData;
    }
}


using System.ComponentModel.DataAnnotations;

namespace VDCoreLib
{
    /// <summary>
    /// <c>VDCoreLib.NullablePagination</c> model is used to add pagination module for method request with the ability
    /// to specify zero values for displaying data without pagination.
    /// </summary>
    /// <remarks>
    /// It is strongly recommended to use a model without the ability
    /// to specify zero and negative values - <c>VDCoreLib.Pagination</c> !!!
    /// </remarks>
    /// <example>
    /// (<c>DataSet</c>) value contains data to paginate,
    /// (<c>request</c>) value contains pagination module reference;
    /// <code>
    /// var enumerable = DataSet.ToList();
    /// var result = enumerable
    ///     .ToPagedList(
    ///         request.RowsOnPage == 0 ? 1 : request.PageNumber,  
    ///         request.RowsOnPage == 0 ? enumerable.Count + 1 : request.RowsOnPage
    ///     );
    /// </code>
    /// </example>
    /// <seealso cref="VDCoreLib.Pagination"/>
    public class NullablePagination
    {
        private int _rowsOnPage;
        private int _pageNumber = 1;
        
        /// <summary>
        /// Max amount of rows displayed on single page.
        /// </summary>
        [Required]
        public int RowsOnPage
        {
            get => _rowsOnPage;
            set => _rowsOnPage = value < 0 ? 0 : value;
        }

        /// <summary>
        /// Current page number.
        /// </summary>
        [Required]
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace VDCoreLib
{
    /// <summary>
    /// <c>VDCoreLib.Pagination</c> model is used to add pagination module for method request.
    /// </summary>
    /// <example>
    /// (<c>DataSet</c>) value contains data to paginate,
    /// (<c>request</c>) value contains pagination module reference;
    /// <code>
    /// return await DataSet.ToPagedListAsync(request.PageNumber, request.RowsOnPage);
    /// </code>
    /// </example>
    /// <exception cref="RowsOnPage"> value can not be less than 1 or null</exception>
    /// <exception cref="PageNumber"> value can not be less than 1 or null</exception>
    public class Pagination
    {
        private int _rowsOnPage;
        private int _pageNumber;
        
        /// <summary>
        /// Max amount of rows displayed on single page.
        /// </summary>
        [Required]
        public int RowsOnPage
        {
            get => _rowsOnPage;
            set
            {
                if (value < 1)
                {
                    throw new Exception("Error! RowsOnPage value can not be less than 1!");
                }
                else
                {
                    _rowsOnPage = value;
                }
            }
        }

        /// <summary>
        /// Current page number.
        /// </summary>
        [Required]
        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (value < 1)
                {
                    throw new Exception("Error! PageNumber value can not be less than 1!");
                }
                else
                {
                    _pageNumber = value;
                }
            }
        }
    }
}
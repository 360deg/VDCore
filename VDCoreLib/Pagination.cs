using System;
using System.ComponentModel.DataAnnotations;

namespace VDCoreLib
{
    public class Pagination
    {
        private int _rowsOnPage;
        private int _pageNumber;
        
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
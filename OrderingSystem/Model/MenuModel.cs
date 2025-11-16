using System.Collections.Generic;
using System.Drawing;

namespace OrderingSystem.Model
{
    public class MenuModel
    {
        public int MenuId { get; protected set; }
        public bool isAvailable { get; protected set; }
        public string MenuName { get; protected set; }
        public string MenuDescription { get; protected set; }
        public string CategoryName { get; protected set; }
        public byte[] MenuImageByte { get; protected set; }
        public Image MenuImage { get; protected set; }
        public int CategoryId { get; protected set; }
        public CategoryModel Category { get; protected set; }
        public DiscountModel Discount { get; protected set; }
        public List<MenuDetailModel> MenuVariant { get; protected set; }
    }
}

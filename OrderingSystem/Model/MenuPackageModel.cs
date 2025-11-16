using System;
using System.Collections.Generic;
using System.Drawing;

namespace OrderingSystem.Model
{
    public class MenuPackageModel : MenuDetailModel
    {
        public bool isFixed { get; protected set; }
        public int PackageId { get; protected set; }
        public List<MenuDetailModel> MenuIncluded { get; protected set; }
        public int Quantity { get; protected set; }
        public interface IMenuPackageBuilder
        {
            MenuPackageBuilder WithMenuImageByte(byte[] image);
            MenuPackageBuilder isAvailable(bool x);
            MenuPackageBuilder WithEstimatedTime(TimeSpan ts);
            MenuPackageBuilder WithDiscount(DiscountModel ts);
            MenuPackageBuilder WithQuantity(int ts);
            MenuPackageBuilder WithPackageId(int ts);
            MenuPackageBuilder WithPackageIncluded(List<MenuDetailModel> included);
            MenuPackageBuilder WithCategory(CategoryModel cat);
            MenuPackageBuilder WithCategoryId(int cat);
            MenuPackageBuilder WithMenuImage(Image image);
            MenuPackageBuilder WithMenuId(int menuId);
            MenuPackageBuilder WithMaxOrder(int menuId);
            MenuPackageBuilder WithMenuName(string menuName);
            MenuPackageBuilder WithFlavorName(string menuName);
            MenuPackageBuilder WithSizeName(string menuName);
            MenuPackageBuilder WithMenuDescription(string menuDescription);
            MenuPackageBuilder WithMenuDetailId(int lowestMenuDetailId);
            MenuPackageBuilder WithPrice(double lowestMenuPrice);
            MenuPackageBuilder isFixed(bool b);
            MenuPackageBuilder WithIngredients(List<IngredientModel> ing);
            MenuPackageBuilder WithCategoryName(string n);
            MenuPackageModel Build();
        }

        public static new MenuPackageBuilder Builder() => new MenuPackageBuilder();
        public class MenuPackageBuilder : IMenuPackageBuilder
        {
            private readonly MenuPackageModel _menuModel = new MenuPackageModel();
            public MenuPackageBuilder WithMenuId(int menuId)
            {
                _menuModel.MenuId = menuId;
                return this;
            }
            public MenuPackageBuilder WithMenuName(string menuName)
            {
                _menuModel.MenuName = menuName;
                return this;
            }
            public MenuPackageBuilder WithMenuDescription(string menuDescription)
            {
                _menuModel.MenuDescription = menuDescription;
                return this;
            }
            public MenuPackageBuilder WithMenuDetailId(int lowestMenuDetailId)
            {
                _menuModel.MenuDetailId = lowestMenuDetailId;
                return this;
            }
            public MenuPackageBuilder WithPrice(double lowestMenuPrice)
            {
                _menuModel.MenuPrice = lowestMenuPrice;
                return this;
            }
            public MenuPackageModel Build()
            {
                return _menuModel;
            }

            public MenuPackageBuilder WithCategory(CategoryModel category)
            {
                _menuModel.Category = category;
                return this;
            }

            public MenuPackageBuilder WithCategoryId(int cat)
            {
                _menuModel.CategoryId = cat;
                return this;
            }


            public MenuPackageBuilder WithMenuImage(Image image)
            {
                _menuModel.MenuImage = image;
                return this;
            }

            public MenuPackageBuilder WithMaxOrder(int max)
            {
                _menuModel.MaxOrder = max;
                return this;
            }

            public MenuPackageBuilder WithFlavorName(string menuName)
            {
                _menuModel.FlavorName = menuName;
                return this;
            }

            public MenuPackageBuilder WithSizeName(string menuName)
            {
                _menuModel.SizeName = menuName;
                return this;
            }

            public MenuPackageBuilder WithEstimatedTime(TimeSpan ts)
            {
                _menuModel.EstimatedTime = ts;
                return this;
            }


            public MenuPackageBuilder isFixed(bool b)
            {
                _menuModel.isFixed = b;
                return this;
            }

            public MenuPackageBuilder WithQuantity(int ts)
            {
                _menuModel.Quantity = ts;
                return this;
            }

            public MenuPackageBuilder WithMenuImageByte(byte[] image)
            {
                _menuModel.MenuImageByte = image;
                return this;
            }

            public MenuPackageBuilder WithIngredients(List<IngredientModel> ing)
            {
                _menuModel.MenuIngredients = ing;
                return this;
            }

            public MenuPackageBuilder WithPackageIncluded(List<MenuDetailModel> included)
            {
                _menuModel.MenuIncluded = included;
                return this;
            }

            public MenuPackageBuilder WithCategoryName(string n)
            {
                _menuModel.CategoryName = n;
                return this;
            }

            public MenuPackageBuilder WithPackageId(int ts)
            {
                _menuModel.PackageId = ts;
                return this;
            }

            public MenuPackageBuilder isAvailable(bool ts)
            {
                _menuModel.isAvailable = ts;
                return this;
            }

            public MenuPackageBuilder WithDiscount(DiscountModel ts)
            {
                _menuModel.Discount = ts;
                return this;
            }
        }
    }
}

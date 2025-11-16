using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OrderingSystem.util;

namespace OrderingSystem.Model
{
    public class MenuDetailModel : MenuModel
    {
        public List<IngredientModel> MenuIngredients { get; protected set; }
        public string FlavorName { get; protected set; }
        public string SizeName { get; protected set; }
        public TimeSpan EstimatedTime { get; protected set; }
        public int MenuDetailId { get; protected set; }
        public double MenuPrice { get; protected set; }
        public int MaxOrder { get; protected set; }

        public override bool Equals(object obj)
        {
            if (obj is MenuDetailModel menu)
            {
                return FlavorName == menu.FlavorName &&
                       SizeName == menu.SizeName &&
                       EstimatedTime == menu.EstimatedTime &&
                       MenuPrice == menu.MenuPrice;
            }
            return false;
        }

        public interface IMenuDetailBuilder
        {
            MenuBuilder WithIngredients(List<IngredientModel> ing);
            MenuBuilder WithVariant(List<MenuDetailModel> m);
            MenuBuilder WithEstimatedTime(TimeSpan ts);
            MenuBuilder WithDiscount(DiscountModel ts);
            MenuBuilder isAvailable(bool ts);
            MenuBuilder WithCategory(CategoryModel cat);
            MenuBuilder WithCategoryId(int cat);
            MenuBuilder WithMenuImage(Image image);
            MenuBuilder WithMenuImageByte(byte[] image);
            MenuBuilder WithMenuId(int menuId);
            MenuBuilder WithMaxOrder(int menuId);
            MenuBuilder WithMenuName(string menuName);
            MenuBuilder WithCategoryName(string n);
            MenuBuilder WithFlavorName(string menuName);
            MenuBuilder WithSizeName(string menuName);
            MenuBuilder WithMenuDescription(string menuDescription);
            MenuBuilder WithMenuDetailId(int lowestMenuDetailId);
            MenuBuilder WithPrice(double lowestMenuPrice);
            MenuDetailModel Build();
        }
        public static MenuBuilder Builder() => new MenuBuilder();
        public double getPriceAfterVat()
        {
            return TaxHelper.VatCalulator(MenuPrice);
        }
        public double getPriceAfterVatWithDiscount()
        {
            return TaxHelper.VatCalulator(MenuPrice - (MenuPrice * (Discount == null ? 0 : Discount.Rate)));
        }
        public MenuDetailModel Clone()
        {
            return Builder()
                .WithMenuId(MenuId)
                .WithMenuName(MenuName)
                .WithMenuDescription(MenuDescription)
                .WithMenuDetailId(MenuDetailId)
                .WithPrice(MenuPrice)
                .WithCategory(Category)
                .WithCategoryId(CategoryId)
                .WithFlavorName(FlavorName)
                .WithSizeName(SizeName)
                .WithEstimatedTime(EstimatedTime)
                .WithMenuImage(MenuImage)
                .WithMenuImageByte(MenuImageByte)
                .WithIngredients(MenuIngredients != null ? new List<IngredientModel>(MenuIngredients) : null)
                .WithVariant(MenuVariant != null ? new List<MenuDetailModel>(MenuVariant.Select(v => v.Clone())) : null)
                .WithCategoryName(CategoryName)
                .WithMaxOrder(MaxOrder)
                .isAvailable(isAvailable)
                .Build();
        }
        public override string ToString()
        {
            return base.ToString();
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public double getPriceAfterDiscount()
        {
            return Math.Round(MenuPrice - (MenuPrice * Discount?.Rate ?? 0), 2);
        }

        public class MenuBuilder : IMenuDetailBuilder
        {
            private readonly MenuDetailModel _menuModel = new MenuDetailModel();
            public MenuBuilder WithMenuId(int menuId)
            {
                _menuModel.MenuId = menuId;
                return this;
            }
            public MenuBuilder WithMenuName(string menuName)
            {
                _menuModel.MenuName = menuName;
                return this;
            }
            public MenuBuilder WithMenuDescription(string menuDescription)
            {
                _menuModel.MenuDescription = menuDescription;
                return this;
            }
            public MenuBuilder WithMenuDetailId(int lowestMenuDetailId)
            {
                _menuModel.MenuDetailId = lowestMenuDetailId;
                return this;
            }
            public MenuBuilder WithPrice(double lowestMenuPrice)
            {
                _menuModel.MenuPrice = lowestMenuPrice;
                return this;
            }
            public MenuDetailModel Build()
            {
                return _menuModel;
            }
            public MenuBuilder WithCategory(CategoryModel category)
            {
                _menuModel.Category = category;
                return this;
            }

            public MenuBuilder WithCategoryId(int cat)
            {
                _menuModel.CategoryId = cat;
                return this;
            }


            public MenuBuilder WithMenuImage(Image image)
            {
                _menuModel.MenuImage = image;
                return this;
            }

            public MenuBuilder WithMaxOrder(int max)
            {
                _menuModel.MaxOrder = max;
                return this;
            }

            public MenuBuilder WithFlavorName(string menuName)
            {
                _menuModel.FlavorName = menuName;
                return this;
            }

            public MenuBuilder WithSizeName(string menuName)
            {
                _menuModel.SizeName = menuName;
                return this;
            }

            public MenuBuilder WithEstimatedTime(TimeSpan ts)
            {
                _menuModel.EstimatedTime = ts;
                return this;
            }

            public MenuBuilder WithIngredients(List<IngredientModel> ing)
            {
                _menuModel.MenuIngredients = ing;
                return this;
            }

            public MenuBuilder WithCategoryName(string n)
            {
                _menuModel.CategoryName = n;
                return this;
            }

            public MenuBuilder WithMenuImageByte(byte[] image)
            {
                _menuModel.MenuImageByte = image;
                return this;
            }

            public MenuBuilder WithVariant(List<MenuDetailModel> m)
            {
                _menuModel.MenuVariant = m;
                return this;
            }

            public MenuBuilder isAvailable(bool ts)
            {
                _menuModel.isAvailable = ts;
                return this;
            }

            public MenuBuilder WithDiscount(DiscountModel ts)
            {
                _menuModel.Discount = ts;
                return this;
            }
        }
    }
}
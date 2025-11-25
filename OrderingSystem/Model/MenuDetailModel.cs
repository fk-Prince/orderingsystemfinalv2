using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OrderingSystem.Model
{
    public class MenuDetailModel
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
        public List<IngredientModel> MenuIngredients { get; protected set; }
        public ServingsModel servingMenu { get; protected set; }


        public interface IMenuDetailBuilder
        {
            MenuBuilder WithIngredients(List<IngredientModel> ing);
            MenuBuilder isAvailable(bool ts);
            MenuBuilder WithCategory(CategoryModel cat);
            MenuBuilder WithCategoryId(int cat);
            MenuBuilder WithMenuImage(Image image);
            MenuBuilder WithMenuImageByte(byte[] image);
            MenuBuilder WithMenuId(int menuId);
            MenuBuilder WithMenuName(string menuName);
            MenuBuilder WithCategoryName(string n);
            MenuBuilder WithMenuDescription(string menuDescription);
            MenuBuilder withServing(ServingsModel serv);
            MenuDetailModel Build();
        }
        public static MenuBuilder Builder() => new MenuBuilder();
        public double getPriceAfterVat()
        {
            return 0;
            // return TaxHelper.VatCalulator(MenuPrice);
        }
        public double getPriceAfterVatWithDiscount()
        {
            return 0;
            //return TaxHelper.VatCalulator(MenuPrice - (MenuPrice * (Discount == null ? 0 : Discount.Rate)));
        }
        public MenuDetailModel Clone()
        {
            return Builder()
                .WithMenuId(MenuId)
                .WithMenuName(MenuName)
                .WithMenuDescription(MenuDescription)
                .WithCategory(Category)
                .WithCategoryId(CategoryId)
                .WithMenuImage(MenuImage)
                .WithMenuImageByte(MenuImageByte)
                .WithIngredients(MenuIngredients != null ? new List<IngredientModel>(MenuIngredients) : null)
                .WithVariant(MenuVariant != null ? new List<MenuDetailModel>(MenuVariant.Select(v => v.Clone())) : null)
                .WithCategoryName(CategoryName)
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
            return 0;
            //return Math.Round(MenuPrice - (MenuPrice * Discount?.Rate ?? 0), 2);
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

            public MenuBuilder withServing(ServingsModel serv)
            {
                _menuModel.servingMenu = serv;
                return this;
            }
        }
    }
}
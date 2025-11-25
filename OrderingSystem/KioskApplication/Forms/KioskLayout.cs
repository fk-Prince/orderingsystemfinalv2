using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySqlConnector;
using OrderingSystem.Exceptions;
using OrderingSystem.KioskApplication;
using OrderingSystem.KioskApplication.Cards;
using OrderingSystem.KioskApplication.Component;
using OrderingSystem.KioskApplication.Components;
using OrderingSystem.KioskApplication.Forms;
using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;
using OrderingSystem.Repository;
using OrderingSystem.Repository.CategoryRepository;
using OrderingSystem.Repository.Coupon;
using OrderingSystem.Repository.Order;
using OrderingSystem.Services;

namespace OrderingSystem
{
    public partial class KioskLayout : Form
    {
        private readonly List<OrderItemModel> orderList;
        private readonly KioskMenuServices menuServicesKiosk;
        private readonly Dictionary<int, FlowLayoutPanel> categoryPanels;
        private readonly Dictionary<int, Guna2Panel> categoryContainer;
        private readonly List<Guna2Button> buttonListTop;
        private Filter filterSide;
        private List<MenuDetailModel> allMenus;
        private CartServices cartServices;
        private CouponModel couponSelected;
        private Guna2Button lastClickedTop;

        private bool isShowing = true;
        private int x = 0;
        private int x1 = 20;
        private int basedx = 0;
        private string type;
        public KioskLayout()
        {
            InitializeComponent();

            orderList = new List<OrderItemModel>();
            menuServicesKiosk = new KioskMenuServices(new KioskMenuRepository(orderList));
            buttonListTop = new List<Guna2Button>();
            categoryPanels = new Dictionary<int, FlowLayoutPanel>();
            categoryContainer = new Dictionary<int, Guna2Panel>();
            cc.Start();
            dt.Start();
            flowMenu.MouseWheel += FlowMenu_MouseWheel;

        }

        public void setType(string type)
        {
            this.type = type;
        }
        private void lastButton(Guna2Button b)
        {
            foreach (var c in buttonListTop)
            {
                if ((int)c.Tag == (int)b.Tag)
                {
                    c.BackColor = Color.Transparent;
                    c.ForeColor = Color.White;
                    c.FillColor = Color.FromArgb(255, 97, 29);
                    lastClickedTop.FillColor = Color.FromArgb(255, 224, 192);
                    lastClickedTop.ForeColor = Color.FromArgb(34, 34, 34);
                    lastClickedTop = c;
                    break;
                }
            }
        }
        private void catClickedTop(object sender, EventArgs e)
        {
            Guna2Button b = sender as Guna2Button;
            int catId = (int)b.Tag;

            foreach (var panel in categoryContainer.Values)
            {
                if (!flowMenu.Controls.Contains(panel))
                    flowMenu.Controls.Add(panel);

            }
            if (categoryPanels.ContainsKey(catId))
            {
                FlowLayoutPanel p = categoryPanels[catId];
                flowMenu.ScrollControlIntoView(p);
            }
            if (lastClickedTop != null && lastClickedTop != b)
            {
                lastButton(b);
            }
            filterSide.resetFilter();
        }
        public void displayMenu(List<MenuDetailModel> mm)
        {
            foreach (var p in categoryPanels.Values)
                p.Controls.Clear();

            if (flowMenu.Controls.Count == 0)
                foreach (var panel in categoryContainer.Values)
                    flowMenu.Controls.Add(panel);

            foreach (MenuDetailModel menu in mm)
            {
                if (categoryPanels.ContainsKey(menu.CategoryId))
                {
                    MenuCard card = new MenuCard(menuServicesKiosk, menu);
                    card.Margin = new Padding(20, 40, 20, 0);
                    card.outOfOrder(menu.servingMenu.LeftQuantity <= 0);
                    card.orderListEvent += (s, e) =>
                    {
                        try
                        {
                            cartServices.addMenuToCart(e);
                            displayTotal(this, EventArgs.Empty);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };
                    categoryPanels[menu.CategoryId].Controls.Add(card);
                }
            }
        }
        public void displayCategory(List<int> id, int price)
        {
            foreach (var kvp in categoryPanels)
            {
                int catId = kvp.Key;
                FlowLayoutPanel panel = kvp.Value;
                bool showCat = (id.Count == 0) || id.Contains(catId);

                if (showCat)
                {
                    panel.Controls.Clear();
                    var menus = allMenus
                        .Where(m => (id.Count == 0 || id.Contains(m.CategoryId))
                                    && m.CategoryId == catId
                                    && m.servingMenu.getPriceAfterVatWithDiscount() <= price)
                        .ToList();

                    foreach (var menu in menus)
                    {
                        MenuCard card = new MenuCard(menuServicesKiosk, menu);
                        card.Margin = new Padding(20, 40, 20, 0);
                        card.outOfOrder(menu.servingMenu.LeftQuantity <= 0);
                        card.orderListEvent += (s, e) =>
                        {
                            cartServices.addMenuToCart(e);
                            displayTotal(this, EventArgs.Empty);
                        };
                        panel.Controls.Add(card);
                    }
                    if (menus.Count > 0 && !flowMenu.Controls.Contains(categoryContainer[catId]))
                        flowMenu.Controls.Add(categoryContainer[catId]);
                }
                else
                {
                    if (flowMenu.Controls.Contains(categoryContainer[catId]))
                        flowMenu.Controls.Remove(categoryContainer[catId]);
                }
            }
        }
        private void displayCategory(List<CategoryModel> cats)
        {

            foreach (CategoryModel c in cats)
            {
                Guna2Panel p = new Guna2Panel();
                p.Width = flowMenu.Width - 40;
                p.Margin = new Padding(20, 20, 20, 20);
                p.MaximumSize = new Size(flowMenu.Width - 40, 10000);
                p.AutoSize = true;
                p.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                if (c.CategoryName == cats.Last().CategoryName)
                    p.Margin = new Padding(20, 20, 20, 40);

                Title title = new Title(c.CategoryName, c.CategoryImage);
                title.BackColor = Color.Transparent;

                p.Controls.Add(title);

                FlowLayoutPanel flowCat = new FlowLayoutPanel();
                flowCat.MaximumSize = new Size(flowMenu.Width - 40, 10000);
                flowCat.AutoSize = true;
                flowCat.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                p.Controls.Add(flowCat);
                flowMenu.Controls.Add(p);

                categoryPanels.Add(c.CategoryId, flowCat);
                categoryContainer.Add(c.CategoryId, p);

                Guna2Button b1 = new Guna2Button();
                b1.Text = c.CategoryName;
                b1.Size = new Size(200, 35);
                b1.Tag = c.CategoryId;
                b1.Margin = new Padding(0);
                b1.Location = new Point(x1, 90);
                b1.AutoRoundedCorners = true;
                b1.FillColor = Color.FromArgb(255, 224, 192);
                b1.Click += catClickedTop;
                b1.BackColor = Color.Transparent;
                b1.ForeColor = Color.FromArgb(34, 34, 34);
                x1 += b1.Size.Width + 5;
                xxx.Controls.Add(b1);
                buttonListTop.Add(b1);
            }

            if (xxx.Controls.Count > 0)
            {
                lastClickedTop = buttonListTop[0];
                lastClickedTop.FillColor = Color.FromArgb(255, 97, 29);
                lastClickedTop.ForeColor = Color.White;
            }
        }
        private void displayTotal(object sender, EventArgs e)
        {
            subtotal.Text = cartServices.calculateSubtotal().ToString("N2");
            cdiscount.Text = cartServices.calculateCoupon(couponSelected).ToString("N2");
            total.Text = cartServices.calculateTotalAmount().ToString("N2");
            orderCount.Text = orderList.Count.ToString();
            count2.Text = orderList.Count.ToString();
        }
        private void couponOption(object sender, EventArgs bx)
        {
            ICouponRepository couponRepository = new CouponRepository();
            CouponFrm c = new CouponFrm(couponRepository, double.Parse(total.Text));
            c.CouponSelected += (s, e) =>
            {
                couponSelected = e;
                displayTotal(this, EventArgs.Empty);
            };
            DialogResult rs = c.ShowDialog(this);
            if (DialogResult.OK == rs)
            {
                displayTotal(this, EventArgs.Empty);
            }
        }
        private void confirmOrder(object sender, EventArgs e)
        {
            try
            {

                if (orderList.Count == 0)
                {
                    MessageBox.Show("No items in the cart.");
                    return;
                }
                IOrderRepository orderRepository = new OrderRepository();
                OrderServices orderServices = new OrderServices(orderRepository);
                string orderId = orderServices.getLastestOrderID();

                OrderModel om;
                if (!string.IsNullOrEmpty(this.type))
                    om = new OrderModel(orderId, orderList, couponSelected, OrderModel.getOrderType(type));
                else
                    om = new OrderModel(orderId, couponSelected, orderList);
                OrderLayout l = new OrderLayout(om, orderServices);
                l.couponSelected += (ss, ee) =>
                {
                    couponSelected = ee;
                    displayTotal(this, EventArgs.Empty);
                };
                l.isBrowsing = string.IsNullOrEmpty(this.type);
                l.AddQuantity += (s, ee) =>
                {

                    foreach (var cc in flowCart.Controls.OfType<CartCard>())
                    {
                        if (cc.menu.PurchaseMenu.servingMenu.ServingId == ee.PurchaseMenu.servingMenu.ServingId)
                        {
                            cartServices.addQuantity(cc, ee);
                            displayTotal(this, EventArgs.Empty);
                            l.refersh();
                            break;
                        }
                    }
                };
                l.DeductQuantity += (s, ee) =>
                {
                    foreach (var cc in flowCart.Controls.OfType<CartCard>())
                    {
                        if (cc.menu.PurchaseMenu.servingMenu.ServingId == ee.PurchaseMenu.servingMenu.ServingId)
                        {
                            cartServices.deductQuantity(cc, ee);
                            displayTotal(this, EventArgs.Empty);
                            l.refersh();
                            break;
                        }
                    }
                };
                l.successfulPayment += (s, ee) =>
                {
                    orderList.Clear();
                    flowCart.Controls.Clear();
                    displayTotal(this, EventArgs.Empty);
                    Dashboard f = this.Parent.Parent as Dashboard;
                    f.reset();
                };
                DialogResult rs = l.ShowDialog(this);
                if (rs == DialogResult.OK)
                {
                    l.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void t_Tick(object sender, EventArgs e)
        {
            if (isShowing)
            {
                x += 20;
                if (x >= this.Width)
                {
                    x = this.Width;
                    isShowing = false;
                    t.Stop();
                }

            }
            else
            {
                x -= 20;
                if (x <= basedx)
                {
                    x = basedx;
                    t.Stop();
                    isShowing = true;
                }

            }
            cartPanel.Location = new Point(x, cartPanel.Location.Y);
        }
        private void KioskLayout_SizeChanged(object sender, EventArgs e)
        {
            x = cartPanel.Location.X;
            basedx = x;
        }
        private void KioskLayout_Load(object sender, EventArgs e)
        {
            try
            {
                CategoryServices categoryServices = new CategoryServices(new CategoryRepository());


                List<CategoryModel> cats = categoryServices.getCategoriesByMenu();
                displayCategory(cats);

                allMenus = menuServicesKiosk.getMenu();
                displayMenu(allMenus);

                double max = 0;
                if (allMenus.Count > 0) max = allMenus.Max(ex => ex.servingMenu.getPriceAfterVatWithDiscount());
                filterSide = new Filter(cats, max);
                catFlow.Controls.Add(filterSide);

                cartServices = new CartServices(menuServicesKiosk, flowCart, orderList);
                cartServices.quantityChanged += (s, b) => displayTotal(this, EventArgs.Empty);
            }
            catch (MaxOrder ex)
            {
                MessageBox.Show(ex.Message, "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Internal Server Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void triggerCart(object sender, EventArgs e)
        {
            t.Stop();
            t.Start();
        }
        private void flowMenuScroll()
        {
            Control onScren = null;
            int top = int.MaxValue;

            bool isBottom = flowMenu.VerticalScroll.Value + flowMenu.ClientSize.Height >= flowMenu.VerticalScroll.Maximum;

            if (isBottom)
            {
                top = int.MinValue;
                foreach (var v in categoryPanels)
                {
                    var panel = v.Value;
                    Rectangle scren = panel.RectangleToScreen(panel.ClientRectangle);
                    Rectangle panelRect = flowMenu.RectangleToClient(scren);
                    bool isVisible = panelRect.Bottom > 0 && panelRect.Top < flowMenu.ClientSize.Height;

                    if (isVisible && panelRect.Top > top)
                    {
                        top = panelRect.Top;
                        onScren = panel;
                    }
                }
            }
            else
            {
                foreach (var kvp in categoryPanels)
                {
                    var panel = kvp.Value;

                    Rectangle screenRect = panel.RectangleToScreen(panel.ClientRectangle);
                    Rectangle panelRect = flowMenu.RectangleToClient(screenRect);

                    bool isVisible = panelRect.Bottom > 0 && panelRect.Top < flowMenu.ClientSize.Height;

                    if (isVisible && panelRect.Top < top)
                    {
                        top = panelRect.Top;
                        onScren = panel;
                    }
                }
            }
            if (onScren != null)
            {
                int categoryId = categoryPanels.FirstOrDefault(v => v.Value == onScren).Key;

                var b = buttonListTop.FirstOrDefault(btn => (int)btn.Tag == categoryId);
                if (b != null && b != lastClickedTop)
                {
                    lastButton(b);
                }
            }
        }
        private void FlowMenu_MouseWheel(object sender, MouseEventArgs e)
        {
            flowMenuScroll();
        }
        private void cc_Tick(object sender, EventArgs e)
        {
            foreach (var btn in buttonListTop)
            {
                int x2 = btn.Location.X - 2;
                if (x2 + btn.Width < 0)
                {
                    int x3 = buttonListTop.Max(b => b.Location.X + b.Width);
                    x2 = x3 + 10;
                }
                btn.Location = new Point(x2, btn.Location.Y);
            }

        }
        private void flowMenu_Scroll(object sender, ScrollEventArgs e)
        {
            flowMenuScroll();
        }
        private void dt_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToString("hh:mm:ss tt");
            date.Text = DateTime.Now.ToString("yyyy, MMMM dd");
            week.Text = DateTime.Now.DayOfWeek.ToString();
        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {
            orderList.Clear();
            flowCart.Controls.Clear();
            displayTotal(this, EventArgs.Empty);
            Dashboard f = this.Parent.Parent as Dashboard;
            f.reset();
        }
    }
}

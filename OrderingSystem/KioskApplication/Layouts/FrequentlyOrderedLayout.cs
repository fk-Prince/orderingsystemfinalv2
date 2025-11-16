using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OrderingSystem.KioskApplication.Components;
using OrderingSystem.Model;

namespace OrderingSystem.KioskApplication
{
    public partial class FrequentlyOrderedLayout : UserControl
    {

        private List<OrderItemModel> checkList;
        public FrequentlyOrderedLayout(List<MenuDetailModel> menus)
        {
            InitializeComponent();
            checkList = new List<OrderItemModel>();

            displayFrequentlyOrdered(menus);
        }
        private void displayFrequentlyOrdered(List<MenuDetailModel> menu)
        {
            int y = 30;

            foreach (var m in menu)
            {
                FrequentlyOrderedCard fot = new FrequentlyOrderedCard(m);
                fot.Location = new Point(20, title.Bottom + y);
                fot.checkedMenu += (s, e) => checkList.Add(e);
                fot.unCheckedMenu += (s, e) => checkList.Remove(e);
                pp.Controls.Add(fot);
                y += 120;
                pp.Height += 110;
                Height += 110;
            }
        }

        public List<OrderItemModel> getFrequentlyOrderList()
        {
            return checkList;
        }
    }
}

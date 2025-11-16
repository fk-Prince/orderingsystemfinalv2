using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OrderingSystem.Model;
using OrderingSystem.Services;


namespace OrderingSystem.KioskApplication.Options
{
    public class FrequentlyOrderedOption : ISelectedFrequentlyOrdered
    {
        private FrequentlyOrderedLayout fot;
        private FlowLayoutPanel flowPanel;
        private KioskMenuServices kioskMenuServices;
        public FrequentlyOrderedOption(KioskMenuServices kioskMenuServices, FlowLayoutPanel flowPanel)
        {
            this.flowPanel = flowPanel;
            this.kioskMenuServices = kioskMenuServices;
        }

        public void displayFrequentlyOrdered(MenuDetailModel menus)
        {
            try
            {
                List<MenuDetailModel> md = kioskMenuServices.getFrequentlyOrderedTogether(menus);

                if (flowPanel.Contains(fot))
                {
                    flowPanel.Controls.SetChildIndex(fot, flowPanel.Controls.Count - 1);
                    return;
                }
                md = md.Where(m => m.MaxOrder >= 2).ToList();
                fot = new FrequentlyOrderedLayout(md);
                fot.Margin = new Padding(20, 30, 0, 20);
                flowPanel.Controls.Add(fot);
                if (flowPanel.Controls.Contains(fot))
                {
                    flowPanel.Controls.SetChildIndex(fot, flowPanel.Controls.Count - 1);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<OrderItemModel> getFrequentlyOrdered()
        {
            return fot?.getFrequentlyOrderList();
        }
    }
}

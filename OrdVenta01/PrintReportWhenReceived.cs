using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace OrdVenta01
{

    class PrintReportWhenReceived
    {
        //
        //Imprime el crystal report directamente sin hacer preview
        //
        private int nvNumero;
        private OrdVenta01.MIKO2016DataSet3 mIKO2016DataSet3;
        private OrdVenta01.MIKO2016DataSet3TableAdapters.nw_nvmovikitTableAdapter mIKO2016DataSet3nw_nvmovikitTableAdapter;
        private OrdVenta01.MIKO2016DataSet3TableAdapters.iw_tprodTableAdapter mIKO2016DataSet3iw_tprodTableAdapter;
        private OrdVenta01.MIKO2016DataSet5 mIKO2016DataSet5;
        private OrdVenta01.MIKO2016DataSet5TableAdapters.ek_PickingGuide1TableAdapter mIKO2016DataSet5ek_PickingGuide1TableAdapter;

        public void PrintReport(int num)
        {
            nvNumero = num;
            mIKO2016DataSet3 = new MIKO2016DataSet3();
            mIKO2016DataSet5 = new MIKO2016DataSet5();

            ReportDocument report = new ReportDocument();   // Report

            //
            // Data del KIT de la tabla nw_movikit
            //
            mIKO2016DataSet3nw_nvmovikitTableAdapter = new MIKO2016DataSet3TableAdapters.nw_nvmovikitTableAdapter();
            mIKO2016DataSet3nw_nvmovikitTableAdapter.FillBy(mIKO2016DataSet3.nw_nvmovikit, nvNumero);
            DataRow[] rowsKit = mIKO2016DataSet3.nw_nvmovikit.Select(
                        null, null, DataViewRowState.CurrentRows);
            //
            //  Data del Producto Kit, se va a buscar solo cuando es KIT
            //
            mIKO2016DataSet3iw_tprodTableAdapter = new MIKO2016DataSet3TableAdapters.iw_tprodTableAdapter();


            //
            //  Data de la vista PickinGuide1 para la Nota de Venta seleccionada 
            //
            mIKO2016DataSet5ek_PickingGuide1TableAdapter = new MIKO2016DataSet5TableAdapters.ek_PickingGuide1TableAdapter();
            mIKO2016DataSet5ek_PickingGuide1TableAdapter.FillBy(mIKO2016DataSet5.ek_PickingGuide1, nvNumero);
            DataRow[] rowsPick = mIKO2016DataSet5.ek_PickingGuide1.Select(
                        null, null, DataViewRowState.CurrentRows);
            foreach (DataRow row in rowsPick)
            {
                if (row["KIT"] == DBNull.Value)
                {
                    row["KIT"] = " ";
                }
                else
                {
                    foreach (DataRow row2 in rowsKit)
                    {
                        Console.WriteLine("rowlinea ={0}", row["nvLinea"]);
                        Console.WriteLine("row2linea={0}", row2["NVLinea"]);
                        if (Convert.ToInt32(row["nvLinea"]) == Convert.ToInt32(row2["NVLinea"]))
                        {
                            //
                            //  Solo si es KIT se vuelve a traer la informacion del codigo del KIT y se almacena en rowProd
                            //
                            mIKO2016DataSet3iw_tprodTableAdapter.FillBy(mIKO2016DataSet3.iw_tprod, Convert.ToString(row2["CodProd"]));
                            DataRow[] rowProd = mIKO2016DataSet3.iw_tprod.Select(
                                        null, null, DataViewRowState.CurrentRows);
                            row["CodProd"] = row2["CodProd"];         // row2 tiene la info del KIT que viene de la tabla nw_movikit
                            row["DesProd"] = row2["DetProd"];
                            row["nvCant"] = row2["NVCant"];
                            row["CodUmed"] = rowProd[0]["CodUmed"];   // rowProd tiene la info del KIT que viene de la tabla iw_tprod
                            row["DesProd2"] = rowProd[0]["DesProd2"];
                        }
                    }
                }
                if (row["FonAux1"] == DBNull.Value)
                {
                    row["FonAux1"] = " ";
                }
                if (row["Usuario"] == DBNull.Value)
                {
                    row["Usuario"] = " ";
                }
                if (row["DesProd2"] == DBNull.Value)
                {
                    row["DesProd2"] = "1";
                }
                else
                {
                    if ((Convert.ToString(row["Desprod2"]).IndexOf(".")) >= 0)
                    {
                        row["Desprod2"] = Convert.ToString(row["Desprod2"]).Replace(".", ",");
                    }
                }

            }
            // mIKO2016DataSet.nw_nventa.AcceptChanges();
            report.Load("../../CrystalReport3.rpt");
            using (mIKO2016DataSet5ek_PickingGuide1TableAdapter)
            {
                report.SetDataSource(from c in mIKO2016DataSet5.ek_PickingGuide1

                                     select new
                                     {
                                         c.NVNumero,
                                         c.nvObser,
                                         c.nvCanalNV,
                                         c.FechaHoraCreacion,
                                         c.nvLinea,
                                         c.nvCant,
                                         c.KIT,
                                         c.NomAux,
                                         c.DirAux,
                                         c.FonAux1,
                                         c.CodProd,
                                         c.DesProd,
                                         c.DesProd2,
                                         c.CodUMed,
                                         c.PesoKgs,
                                         c.Usuario,
                                         c.UsuarioGeneraDocto
                                     });
            }
            // report.SetDataSource(rowsPick);

            //crystalReportsViewer1.ViewerCore.ReportSource = report;
            try
            {
                System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
                report.PrintOptions.PrinterName = printDocument.PrinterSettings.PrinterName;
                report.PrintToPrinter(1, true, 0, 0);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using e3;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            
        }

        public void AddToListBox()
        {
            app = ConnectToE3();
            job = CreateJobObject();
            dev = job.CreateDeviceObject();
            dynamic devids1=null;
            dynamic devids2=null;
            
            int nDev1 = job.GetSelectedDeviceIds(ref devids1);
            int nDev2 = job.GetTreeSelectedDeviceIds(ref devids2);
            int  [] slctDevs=new int [nDev1+nDev2];
            for (int i = 0; i < nDev1; i++)
            {
                slctDevs[i] = devids1[i+1]; 
            }
            for (int k = 0; k < nDev2; k++)
            {
                slctDevs[k + nDev1] = devids2[k+1];
            }

            string[] myList = new string[(nDev1+nDev2)];
            for (int j = 0; j < nDev1+nDev2; j++)
            {
                dev.SetId(slctDevs[j]);
                myList[j] = "" + dev.GetComponentName();
            }
            listBox1.Items.AddRange(myList);
            
            
            app = null;
            job = null;
            dev = null;
        }
        

        e3Application app;
        e3Job job;
        e3Device dev;
        e3Device dev1;
        e3Device dev2;
        e3Symbol sym;
        e3Sheet sht;
        e3Pin pin;
        e3Pin pin1;
        e3Pin pin2;
        e3Pin cor;
        e3Text txt;
        e3Job CreateJobObject()
        {
            // возвращаемое функцией значение бла бла бла бла 
            e3Job ret = null;
            try
            {
                ret = (e3Job)app.CreateJobObject();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Невозможно создать объект Job в E3.Series" +
                    "\nСообщение: " + ex.Message +
                    "\n\nВыход из программы");
                System.Environment.Exit(0);
            }

            // нет открытых проектов - выход
            if (ret.GetId() == 0)
            {
                MessageBox.Show("Нет открытых проектов" +
                    "\n\nВыход из программы");
                System.Environment.Exit(0);
            }

            return ret;
        }
        e3Application ConnectToE3()
        {
            // получаем количество процессов E3.series
            int processCount =
                System.Diagnostics.Process.GetProcessesByName("E3.series").Length;

            switch (processCount)
            {
                case 0: // нет запущенных процессов
                    MessageBox.Show("Нет запущенных окон E3.Series" +
                        "\n\nВыход из программы");
                    System.Environment.Exit(0);
                    break;
                case 1: // запущен один процесс
                    try
                    {
                        return (e3Application)System.Activator.CreateInstance
                            (System.Type.GetTypeFromProgID("CT.Application"));
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("Не установлен E3.Series" +
                            "\n\nВыход из программы");
                    }
                    break;
                default: // запущено несколько процессов
                    dynamic ret = null;

                    try
                    {
                        dynamic Viewer = System.Activator.CreateInstance
                        (System.Type.GetTypeFromProgID("CT.DispatcherViewer"));
                        Viewer.ShowViewer(ref ret);
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("Открыто несколько окон E3.Series\n" +
                            "Закройте 'лишние' окна или установите E3.Dispatcher" +
                            "\n\nВыход из программы");
                    }

                    if (ret == null)
                    {
                        MessageBox.Show("Не выбрано окно E3.Series" +
                            "\n\nВыход из программы");
                        System.Environment.Exit(0);
                    }
                    else return (e3Application)ret;
                    break;
            }

            return null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.IndexOf(listBox1.SelectedItem);
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddToListBox();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
           
        }

        
    }

}

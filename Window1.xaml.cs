using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace WpfTreeView
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 










  
    public partial class Window1 : Window
    {
        int cntForList;
        object lastMoveing;
        List<string> words = new List<string>();
        Point _lastMouseDown;
        TreeViewItem draggedItem, _target;
        public Window1()
        {
            InitializeComponent();
        }




        //调用python



        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //命令写这里就行了.



// python代码写入................................
            string[] lines = { "print(111111111)", "print(222222)", "print(333333333)" };

            System.IO.File.WriteAllLines(@"../../1.py", lines);


            //运行python代码
            string str = "cd ../ &cd ../ & python 1.py";
            Console.WriteLine("ffffffffffff");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}



            //下面对结果进行加急. 修复      output

            for (int i = 0; i < 4; i++) { int aaa=output.IndexOf("\r\n");
                output = output.Substring(aaa+2);
            }

            for (int i = 0; i <1; i++)
            {
                int aaa = output.LastIndexOf("\r\n");
                output = output.Substring(0,aaa );
            }











            p.WaitForExit();//等待程序执行完退出进程
            p.Close();


            Console.WriteLine("下面我们打印调用的结果是什么!!!!!!!!!!!!");
            Console.WriteLine(output);        //返回的结果都在output里面.........!!!!!!!!!
            Console.WriteLine("打印完毕!!!!!!!!!!!!");
        }
     




























        /////*  */  拖拽处理函数. a
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {   //处理点击函数.





            Console.WriteLine(sender.GetType().ToString());// 点了什么OriginalSource里面就存了什么东西.
            Console.WriteLine(sender.GetType().ToString());// 点了什么OriginalSource里面就存了什么东西.
            Console.WriteLine(sender.GetType().ToString());// 点了什么OriginalSource里面就存了什么东西.
            Console.WriteLine(sender.GetType().ToString());// 点了什么OriginalSource里面就存了什么东西.
            TextBlock obj = (TextBlock)e.OriginalSource;//转化一下类型.
            DragDrop.DoDragDrop(obj, obj.Text, DragDropEffects.Copy);
        }


        //  处理drop函数. 扔过来的时候, e.data.
        private void listBox_Drop(object sender, DragEventArgs e)
        {
            try
            {
                Console.WriteLine("debug, 99999999999");
                Console.WriteLine(e.Handled);
                Console.WriteLine(sender);
                Console.WriteLine("debug, listBox_Drop");
                Console.WriteLine(e.OriginalSource);
                Console.WriteLine(e.Data);
                Console.WriteLine(e.Data.GetData(DataFormats.Text));

          
                   string   data = e.Data.GetData(DataFormats.Text).ToString();
        
           
               
                e.Data.GetData(DataFormats.UnicodeText);
                words.Add(data);
                Label aaa = new Label();// 设置一个id即可. 达到索引的目的.
                aaa.Content = data;
                aaa.DragEnter += lblTarget_DragEnter_1;
                aaa.Drop += lblTarget_Drop_1;
                aaa.MouseDown += Label_MouseDown_1;
                aaa.AllowDrop = true;
                aaa.Name = "pre"+ cntForList.ToString();
               
                cntForList+=1;
                listBox.Items.Add(aaa);
                Console.WriteLine("当前轮");
                foreach (string s in words) Console.WriteLine(s);
            }
            catch(Exception e333) { }
        }

        private void Label_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            lastMoveing = sender;  //sender.Parent
           
            
   //找到他再他parent里面的索引.
            Console.WriteLine(sender);
            DragDrop.DoDragDrop(this, this.Content, DragDropEffects.Copy);
        }






        private void lblTarget_Drop_1(object sender, DragEventArgs e)







        {


            //下面我们加入坐标比较, 来实现上插入和下插入.

            




            //sender是目标时间,   e里面存储了 source物品.
            Console.WriteLine("进入右边drop函数中了.下一个是鼠标进入的物品");


            // debug找到对应的索引.
            System.Windows.Controls.Label sender2 = (System.Windows.Controls.Label)sender as System.Windows.Controls.Label;       //必须强制转化一下才得到真真的name. c#就这样.比价傻.

            string aaa4 = "";
            aaa4 = ((sender2 as System.Windows.Controls.Label).Name);


            Point p = e.GetPosition((IInputElement)sender);
            
            Console.WriteLine(7777777);
            Console.WriteLine(p);
            Console.WriteLine("鼠标在控件中的相对坐标在上一行");//

            Console.WriteLine(sender2.ActualHeight);
            Console.WriteLine("控件高度在上一行");

            //所以比较
            int shang = 0;
            if (p.Y>= sender2.ActualHeight/2)
            {
                shang = 1;
            }













            Console.WriteLine("得到source东西是什么,鼠标从哪里来的");

            string aaa3 = ((lastMoveing as System.Windows.Controls.Label).Name);


            Console.WriteLine(lastMoveing);
            Console.WriteLine("来的索引是");
            Console.WriteLine(aaa3);

            e.Handled = true;
            Console.WriteLine(e.Handled);

            // 下面计算控件的坐标. 然后fix parent即可.
            
            System.Windows.Controls.ItemCollection item = listBox.Items;


            //if (item.Parent != null) item.Parent.SetValue(ContentPresenter.ContentProperty, null);

            ArrayList myAL = new ArrayList();
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                myAL.Add(listBox.Items[i]);// 根据Name进行排序.先扔着
                // 遇到aaa4 就 在她前面放入aaa3 的索引即可.
            }
            ArrayList myAL2 = new ArrayList();
            int memo=0;
            if (myAL.Count > 1)
            {

                //找到memo
                for (int i = 0; i < listBox.Items.Count; i++)
                {
                    if ((listBox.Items[i] as System.Windows.Controls.Label).Name == aaa3) { memo = i;break; }
             
                }



                Console.WriteLine("打印一下我们的判断函数hi");
                Console.WriteLine(shang);

                if (shang == 0)
                {
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if ((listBox.Items[i] as System.Windows.Controls.Label).Name == aaa4) { myAL2.Add(listBox.Items[memo]); }//把aaa3 插入aaa4前面了.

                        if ((listBox.Items[i] as System.Windows.Controls.Label).Name == aaa3) { continue; };
                        myAL2.Add(listBox.Items[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if ((listBox.Items[i] as System.Windows.Controls.Label).Name == aaa3) { continue; };
                        myAL2.Add(listBox.Items[i]);

                        if ((listBox.Items[i] as System.Windows.Controls.Label).Name == aaa4) { myAL2.Add(listBox.Items[memo]); }//把aaa3 插入aaa4后了.

                       
               
                    }

                }










            }

            if (listBox.Items.Count > 0)
                listBox.Items.Clear();

            for (int i = 0; i < myAL2.Count; i++)
            {
                listBox.Items.Add(myAL2[i]);
                
            }


           Console.WriteLine();



        }

        private void lblTarget_DragEnter_1(object sender, DragEventArgs e)
        {
            Console.WriteLine("lblTarget_DragEnter_1");
            Console.WriteLine(e.Handled);
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
        }






        private void treeView_Drop222(object sender, DragEventArgs e)
        {
            Console.WriteLine(e.Handled);
            Console.WriteLine("test111111");
        }










































            private void treeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                TreeViewItem TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (TargetItem != null && draggedItem != null )
                {
                    _target = TargetItem;
                    e.Effects = DragDropEffects.Move;

                }
            }
            catch (Exception)
            {
            }



        }






  








        private bool CheckDropTarget(TreeViewItem _sourceItem, TreeViewItem _targetItem)
        {
            //Check whether the target item is meeting your condition
            bool _isEqual = false;
            if (!_sourceItem.Header.ToString().Equals(_targetItem.Header.ToString()))
            {
                _isEqual = true;
            }
            return _isEqual;

        }



        private TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }

        public void addChild(TreeViewItem _sourceItem,TreeViewItem _targetItem)
        {
            // add item in target TreeViewItem 
            TreeViewItem item1 = new TreeViewItem();
            item1.Header = _sourceItem.Header;
            _targetItem.Items.Add(item1);    
            foreach (TreeViewItem item in _sourceItem.Items)
            {
                addChild(item, item1);               
            }
        }

















        static TObject FindVisualParent<TObject>(UIElement child) where TObject : UIElement
        {
            if (child == null)
            {
                return null;
            }

            UIElement parent = VisualTreeHelper.GetParent(child) as UIElement;

            while (parent != null)
            {
                TObject found = parent as TObject;
                if (found != null)
                {
                    return found;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent) as UIElement;
                }
            }

            return null;
        }
  

    }
}

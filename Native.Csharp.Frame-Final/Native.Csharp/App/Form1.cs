using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Native.Csharp.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MomordicaMain mmdk;

        void print(string str)
        {
            if (this.InvokeRequired)
            {
                Invoke(new EventHandler(delegate {
                    print(str);

                }));
            }
            else
            {
                textBox1.AppendText(str + Environment.NewLine);
            }
        }



        private void log(string str)
        {
            tryInit();
            print($"log:{str}");
        }

        private void tryInit()
        {
            try
            {
                print("正在初始化……");
                if (mmdk == null)
                {
                    mmdk = MomordicaMain.getMomordicaMain();
                    mmdk.config.myQQ = 2;// GetLoginQQ();
                    mmdk.rootDict = "./RunningData/";
                    mmdk.log = log;
                    mmdk.sendGroup = sendGroup;
                    mmdk.sendPrivate = sendPrivate;
                    mmdk.getQQNick = getQQNick;
                    mmdk.getQQNickFromGroup = getQQNickFromGroup;
                    mmdk.getQQNumFromGroup = getQQNumFromGroup;
                    mmdk.getQQGroupNum = getQQGroupNum;
                    mmdk.getQQImage = getQQImage;
                    mmdk.tryInit();

                }
                print($"{mmdk.config.askName}启动完成。");
                changeBkg(mmdk.config.bkgFile);
                changeAvatar(mmdk.config.avatarFile);

            }
            catch (Exception ex)
            {
                print($"初始化出错：{ex.Message}");
            }
        }

        private void sendPrivate(long user, string msg)
        {
            if (mmdk.config.ignoreall)
            {
                return;
            }
            SendPrivateMessage(user, msg);
        }

        private void SendPrivateMessage(long user, string msg)
        {

            print($"[{user}]{msg}");
        }

        private string getQQNick(long qq)
        {
            try
            {
                if(qq==1)return "你";

                return qq.ToString();// GetQQInfo(qq).Nick;
            }
            catch (Exception e)
            {
                return qq.ToString();
            }
        }



        private string getQQImage(string msg)
        {
            try
            {
                return "";// ReceiveImage(msg);
            }
            catch (Exception e)
            {
                return "";
            }
        }

        private string getQQNickFromGroup(long group, long qq)
        {
            try
            {
                return "";// GetMemberInfo(group, qq).Nick;
            }
            catch (Exception e)
            {
                return qq.ToString();
            }
        }

        private long getQQNumFromGroup(long group, string nick)
        {
            try
            {
                //var list = GetMemberList(group);
                //foreach (var line in list)
                //{
                //    //log($"user:{line.QQId}  {line.Nick}  {line.Level}");
                //    if (line.Nick.Trim().ToUpper() == nick.Trim().ToUpper()) return line.QQId;
                //}
                return -1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        private long getQQGroupNum()
        {
            try
            {
                return 1;// GetGroupList().Count;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        private void sendGroup(long group, long user, string msg)
        {
            if (mmdk.config.ignoreall)
            {
                return;
            }

            int maxlen = 1600;
            int maxt = 5;
            msg = mmdk.config.replaceSSTV(msg);
            do
            {
                maxt -= 1;
                try
                {
                   
                    string tmp = msg.Substring(0, Math.Min(msg.Length, maxlen));

                    //if (user > 0)
                    //{
                    //    tmp = $"@{user} {tmp}";// GetMemberInfo(group, user).Nick + " " + msg;// CqCode_At(user) + msg;

                    //}
                    //if (mmdk.config.useGroupMsgBuf > 0)
                    //{
                    //    tmp = "\r\n" + tmp;
                    //    for (int i = 0; i < mmdk.config.useGroupMsgBuf; i++)    // 33  54
                    //    {
                    //        tmp = CqCode_Face(Sdk.Cqp.Enum.Face.拳头) + tmp;
                    //    }
                    //}
                    SendGroupMessage(group, tmp);

                    msg = msg.Substring(Math.Min(msg.Length, maxlen));
                }
                catch { }

                if (maxt <= 0) break;
            } while (msg.Length > 0);


        }

        private void SendGroupMessage(long group, string tmp)
        {
            print($"[{mmdk.config.askName}]{tmp}");
        }

        public void ReceiveGroupMessage(object sender, CqGroupMessageEventArgs e)
        {
            tryInit();
            try
            {
                mmdk.dealGroupMsg(e.FromGroup, e.FromQQ, e.Message);
            }
            catch (Exception ex)
            {
                sendPrivate(mmdk.config.masterQQ, ex.Message + "\r\n" + ex.StackTrace);
            }

        }

        void workDealMessage(object str)
        {
            mmdk.dealGroupMsg(1, 1, (string)str);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string str = textBox2.Text;
            if (str.EndsWith("\n"))
            {
                str = str.TrimEnd();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    print($"[你]{str}");
                    new Thread(workDealMessage).Start((object)str);
                }
                textBox2.Clear();
            }
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
            new Thread(tryInit).Start();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        void changeBkg(string file)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(file))
                {
                    BackgroundImage = null;
                }
                else
                {
                    var img = Image.FromFile(file);
                    double formpct = (double)(Width + 0) / (Height - 50);
                    double pct = (double)img.Width / img.Height;
                    int neww, newh;
                    int srcw, srch;
                    if (pct >= formpct)
                    {
                        newh = (Height - 50);
                        neww = (int)(newh * pct);
                        srch = img.Height;
                        srcw = (int)(srch * pct);
                    }
                    else
                    {
                        neww = (Width + 0);
                        newh = (int)(neww / pct);
                        srcw = img.Width;
                        srch = (int)(srcw / pct);
                    }
                    Image imgn = new Bitmap(neww, newh);
                    Graphics g = Graphics.FromImage(imgn);
                    g.DrawImage(img, new Rectangle(0, 0, neww, newh), new Rectangle(0, 0, srcw, srch), GraphicsUnit.Pixel);
                    BackgroundImage = imgn;
                }
                
                mmdk.config.bkgFile = file;
                mmdk.config.save();
            }
            catch (Exception ex)
            {
                print($"换背景出错：{ex.Message}");
            }
        }

        void changeAvatar(string file)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(file))
                {
                    // default
                    pictureBox1.Image = Properties.Resources.mmdk;
                    mmdk.config.avatarFile = "";
                }
                else
                {
                    pictureBox1.Image = Image.FromFile(file);
                    mmdk.config.avatarFile = file;
                }
                mmdk.config.save();
            }
            catch (Exception ex)
            {
                print($"换立绘出错：{ex.Message}");
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            changeAvatar(openFileDialog1.FileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            changeAvatar("");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            changeBkg(openFileDialog2.FileName);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(-1);
        }

        //public void ReceiveFriendMessage(object sender, CqPrivateMessageEventArgs e)
        //{
        //    tryInit();
        //    try
        //    {
        //        mmdk.dealPrivateMsg(e.FromQQ, e.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        sendPrivate(mmdk.config.masterQQ, ex.Message + "\r\n" + ex.StackTrace);
        //    }
        //}

        //public void ReceiveAddGroupBeInvitee(object sender, CqAddGroupRequestEventArgs e)
        //{
        //    SetGroupAddRequest(e.ResponseFlag, Sdk.Cqp.Enum.RequestType.GroupInvitation, Sdk.Cqp.Enum.ResponseType.PASS, "");
        //}

        //public void ReceiveFriendAddRequest(object sender, CqAddFriendRequestEventArgs e)
        //{
        //    SetFriendAddRequest("", Sdk.Cqp.Enum.ResponseType.PASS);
        //}

        //public void ReceiveFriendIncrease(object sender, CqFriendIncreaseEventArgs e)
        //{
        //    //sendPrivate(e.FromQQ, mmdk.getWelcomeString());
        //}

        //public void ReceiveGroupPrivateMessage(object sender, CqPrivateMessageEventArgs e)
        //{
        //    tryInit();
        //    try
        //    {
        //        mmdk.dealPrivateMsg(e.FromQQ, e.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        sendPrivate(mmdk.config.masterQQ, ex.Message + "\r\n" + ex.StackTrace);
        //    }
        //}
    }

    public class CqGroupMessageEventArgs
    {
        public long FromGroup { get; internal set; }
        public long FromQQ { get; internal set; }
        public string Message { get; internal set; }
    }



}

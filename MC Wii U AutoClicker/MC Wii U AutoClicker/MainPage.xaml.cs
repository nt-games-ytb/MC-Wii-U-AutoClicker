using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using MC_Wii_U_AutoClicker;
using geckou;
using System.IO;
using System.Threading;

namespace MC_Wii_U_AutoClicker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            labelTimeBetweenClickZL.Text = TimeBetweenClick + timeBetweenClickZL.Value * 0.05 + " s";
            labelTimeBetweenClickZR.Text = TimeBetweenClick + timeBetweenClickZR.Value * 0.05 + " s";
        }

        public static GeckoUCore GeckoU;
        public static GeckoUConnect GeckoUConnection;
        public static GeckoUDump GeckoUDump;
        string nintendoNetwork;
        string TimeBetweenClick = "Time between click : ";

        #region Connection
        #region Text
        public bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            if (ipString == "127.0.0.1")
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');

            if (splitValues.Length != 4)
            {
                return false;
            }

            return splitValues.All(r => byte.TryParse(r, out byte tempForParsing));
        }

        public void ipText_TextChanged(object sender, EventArgs e)
        {
            if (ValidateIPv4(ipText.Text) == true)
            {
                connect.IsEnabled = true;
            }
            else
            {
                connect.IsEnabled = false;
            }
        }
        #endregion

        #region Button
        private async void connect_Clicked(object sender, EventArgs e)
        {
            try
            {
                GeckoU = new GeckoUCore(ipText.Text);
                GeckoU.GUC.Connect();

                GetNintendoNetwork();
                await DisplayAlert("Minecraft Wii U AutoClicker", "Welcome " + nintendoNetwork.Replace("/0", "") + ",\r\nyou are well connected to Minecraft Wii U AutoClicker !", "OK");

                //Old AutoClick ZL : GeckoU.makeAssembly(0x03C00000, "9421FFF87C0802A63DE000003C8010A061EF00006084A628808400008084009C6000000091E409A83DE000803C8010A961EF000060840E6C8084000091E400183C00010F60006AE07C0903A64E80002060000000");
                
                if (GeckoU.PeekUInt(0x03FFFFF8) == 1)
                { }
                else
                {
                    string code1 = "3C401002604250009002000090220004904200089062000C9082001090A2001490C2001890E2001C9102002091220024914200289162002C9182003091A2003491C2003891E2003C9202004092220044924200489262004C9282005092A2005492C2005892E2005C9302006093220064934200689362006C9382007093A2007493C2007893E2007CD0020080D0220084D0420088D062008CD0820090D0A20094D0C20098D0E2009CD10200A0D12200A4D14200A8D16200ACD18200B0D1A200B4D1C200B8D1E200BCD20200C0D22200C4D24200C8D26200CCD28200D0D2A200D4D2C200D8D2E200DCD30200E0D32200E4D34200E8D36200ECD38200F0D3A200F4D3C200F8D3E200FC480001253C401002604250008002000080220004804200088062000C8082001080A2001480C2001880E2001C8102002081220024814200288162002C8182003081A2003481C2003881E2003C8202004082220044824200488262004C8282005082A2005482C2005882E2005C8302006083220064834200688362006C8382007083A2007483C2007883E2007CC0020080C0220084C0420088C062008CC0820090C0A20094C0C20098C0E2009CC10200A0C12200A4C14200A8C16200ACC18200B0C1A200B4C1C200B8C1E200BCC20200C0C22200C4C24200C8C26200CCC28200D0C2A200D4C2C200D8C2E200DCC30200E0C32200E4C34200E8C36200ECC38200F0C3A200F4C3C200F8C3E200FC3C4010506042A2003D80031A618C44F87D8903A64E8004203C2012007C0802A69001000048000044480001784800019C480001C0480001E44800022048000240480002604800029448000380480003B84800061448000A6C3C201200800100007C0803A64BFFFE953C2011003C40102E6042F7C0804200002F820000419E01203C40102F60426B40C1420000C16200043C40450060422000904101103C4044696042C000904101183C40BF80604200009041011C3C40BFB96042999A904101203C403FB96042999A904101143C403E996042999A90410124C1E10110FD4A7828C1E10114FD4A7824C1E10118FD6B7828C1E1011CFD6B03F2C1E10120FD6B7824D1410130D16101343C4010A06042A61080620000806301042F830000419E00803C4042B4604200009043014C3C404334604200009043014880630158C3210124FD595028D1410140FD59502AFD59502AD1410144FD795828D1610148FD79582AFD79582AD161014CC1E1011CC1410140FD4F02B2D9430000C1410144FD4F02B2D9430018C1610148FD6F02F2D9630010C161014CFD6F02F2D96300284BFFFE8C3C6010A06063A61080630000806301042F830000419EFE783C403CA36042D70A904303F04BFFFE683C6010A06063A61080630000806301042F830000419EFE543C400101604201019043015C4BFFFE443C6010A06063A61080630000806301042F830000419EFE303C4010009043013D9043012F4BFFFE203C6010A06063A61080630000806301042F830000419EFE0C384000002F820001419E001038400000904301DA4BFFFDF43C40010160420101904301DA4BFFFDE43C6010A06063A61080630000806301042F830000419EFDD038400004904309104BFFFDC43C6010A06063A61080630000806301042F830000419EFDB038400000904308F84BFFFDA43C6010A06063A61080630000806301042F830000419EFD908043070C2F820000419E00084BFFFD803C400100604201019043070C4BFFFD703C6010A06063A61080630000806301042F830000419EFD5C3C2011003C40102F60426A8080420000704308002C020800704404002C020400704502002C020200704601002C0201007C631BD67C8423D67CA52BD67CC633D63CE040403D00C0403D2040403D40C0407CE719D67D0821D67D2929D67D4A31D690E1000091010004912100089141000CC0E10000C1010004C1210008C141000C3D6010A0616BA610816B0000816B0104816B0158C98B0000C9AB0018C9CB0010C9EB0028FD87602AFDA7682AFD88602AFDA8682AFDC9702AFDE9782AFDCA702AFDEA782AD98B0000D9AB0018D9CB0010D9EB00284BFFFC843C60109C6063D8E480630000806300042F830000419EFC70384000009043002C904300283C4001009043000C3C40FFFF6042FFFF904300244BFFFC4C3C20110060210D003C6010A06063A61080630000806301042F830000419E01E8804100042F820000419E01883C80109C6084D8E4808400008084002C2F840000419E01C4804100042F820000419E016480A4003880C4003C2F850000419E01A8804100041C4200087C8512147F843040419E0194409C019080840000C9430118C963012880A30158C9C50020C9840118C9A4012880A40158C9E50020FD8A6028FDAB6828FDCF7028D1810030D1A10034D1C100383C2011086021F0003D800383618C310CFC406090FC2068907D8903A64E8004213C20110038210D003C40433490410040C0410040FC2200723C4040486042F5C390410040C0410040FC2110243C4042B490410040C0410040FC22082AFC200818D0230148C1810030C1A10034FD8C0332FDAD0372FC2D602A3C2011086021F1283D800383618C23CC7D8903A64E8004213C20110060210D00C04100383C2011086021F0003D800383618C310C7D8903A64E8004213C20110038210D003C40433490410040C0410040FC2200723C4040486042F5C390410040C0410040FC2110243C4042B490410040C0410040FC220828FC200818FC200850D023014C3C20110038210D00804101007C4803A63C40102E6042FA6480420000704208002F820800419E00283C40102E6042FA6480420000704202002F820200419E003C3840000090410000480000643840000090410008804100082F820001419E00103840000090410004480000443840000190410004480000383840000190410008818100002F8C0001419EFFC0398000017D8903A638400001904100008041000438420001904100044200FFEC4BFFF9F0480003492F830001419E045048000009480004487C220B783C20110060210DF0904100047C4802A6904100003C80109C6084D8E4808400008084002C2F83000080A4003880C4003C7CA5305038C000087CA533D62F850001419E0020384000012F820000419E00147F822840409C000C480001ED4800002D3C20110060210E7038400000904100143C20110060210DF080410000802100047C4803A64E8000207C220B783C20110060210ED0904100007C4802A6904100043C20110060210E70808100142F8400003C20110060210ED0419E017C3C20110060210ED03C6010A06063A6108063000080630104C9430118C963012880A30158C9850020D181002080A40158C9850020D1810024C9840118C9A40128FD8A6028FDAB6828D1810028D1A1002C3C2011086021F0003D800383618C310CFC406090FC2068907D8903A64E8004213C20110060210ED03C40433490410030C0410030FC2200723C4040486042F5C390410030C0410030FC2110243C4042B490410030C0410030FC22082AFC200818D02301483C20110060210ED0C1810028C1A1002CC1410020C1610024FD4B5028D1410038FD8C0332FDAD0372FD8D602AFC2060903C2011086021F0003D800383618C23CC7D8903A64E8004213C20110060210ED0C04100383C2011086021F0003D800383618C310C7D8903A64E8004213C20110060210ED03C40433490410030C0410030FC2200723C4040486042F5C390410030C0410030FC2110243C4042B490410030C0410030FC220828FC200818FC2008503C6010A06063A6108063000080630104D023014C80410004802100007C4803A64E8000207C220B783C20110060210E50904100107C4802A6904100009081000490A100083C6010A06063A61080630000806301042F830000419E009C9061000C7CBD2B787C9E23787C7F1B783C20110060210E7093C1000493A1000838400001904100003C40447A9041001038400000904100143C20110060210E70804100008081000480A100087F822840409C00401C42000880810004808400387C8222148084000048000075C0410010FF811000409C000CD0210010908100148041000038420001904100004BFFFFAC3C20110060210E5080A10008808100048061000C80410000802100107C4803A64E8000203C6010A06063A61080630000806301042F83000038600000419E00084E800020386000014E8000207C220B783C20110060210EA0904100007C4802A690410004906100089081000CC9430118C963012880630158C9840118C9A40128808401589061001090810014FD4C5028FD6D5828D1410018D161001CFD8A02B2FDAB02F2FD8C682A3C2011086021F0003D800383618C23CC7D8903A6FC2060904E8004213C20110060210EA0FD4008908061001080810014C9630020C9840020FD6C5828D1610018D181001CFD8A02B2FDAB02F2FD8D602A3C2011086021F0003D800383618C23CC7D8903A6FC2060904E8004213C20110060210EA08081000C8061000880410004802100007C4803A64E8000204BFFF5983C201100602120003C403C036042126F90410004C0410004C0210000FC22082AD02100003C403F8090410004C0410004C0210000FF811000409C00084800000C3840000090410000C02100003C403F8090410004C04100043C403F8090410004C06100043D800262618C41787D8903A64E8004213C201000906100004BFFF51C";
                    string code2 = "7C0802A63D801200804C001038420001904C00102F822400409C00084800001C380000004400000238007C004400000238000001440000023D8002F1618CA4F07D8903A64E800420";
                    GeckoU.makeAssembly(0x04000000, code1);
                    GeckoU.makeAssembly(0x04400000, code2);

                    GeckoU.WriteUInt(0x04000238, 0x60000000);
                    GeckoU.WriteUInt(0x0400023C, 0x60000000);
                    GeckoU.WriteUInt(0x04000240, 0x60000000);
                    GeckoU.WriteUInt(0x04000244, 0x60000000);
                    GeckoU.WriteUInt(0x04000248, 0x480001E4);
                    GeckoU.WriteUInt(0x0400024C, 0x60000000);
                    GeckoU.WriteUInt(0x04000250, 0x60000000);
                    GeckoU.WriteUInt(0x04000254, 0x60000000);
                    GeckoU.WriteUInt(0x04000258, 0x60000000);
                    GeckoU.WriteUInt(0x0400025C, 0x60000000);
                    GeckoU.WriteUInt(0x04000260, 0x60000000);
                    GeckoU.WriteUInt(0x04000264, 0x60000000);
                    GeckoU.WriteUInt(0x04000268, 0x48000A6C);
                    GeckoU.WriteUInt(0x03FFFFFC, 0x7C0802A6);
                    GeckoU.WriteUInt(0x03FFFFF8, 0x00000001);
                }
                GeckoU.WriteUInt(0x031A44F4, 0x48E5BB08);

                ipText.IsEnabled = false;
                connect.IsEnabled = false;
                disconnect.IsEnabled = true;
                autoClickZL.IsEnabled = true;
                autoClickZR.IsEnabled = true;
                toggleSrint.IsEnabled = true;
                showArmor.IsEnabled = true;
                showHitbox.IsEnabled = true;
                hitboxSize.IsEnabled = true;
                macro.IsEnabled = true;

            }
            catch (IOException ex)
            {
                await DisplayAlert("Minecraft Wii U AutoClicker", ex.Message, "OK");
            }
            catch (System.Net.Sockets.SocketException)
            {
                await DisplayAlert("Minecraft Wii U AutoClicker", "Error: your ip is not the right one or you are not connected to the internet!", "OK");
            }
            catch
            {
                await DisplayAlert("Minecraft Wii U AutoClicker", "An unknown error has occurred !", "OK");
            }
        }

        private void disconnect_Clicked(object sender, EventArgs e)
        {
            GeckoU.GUC.Close();

            ipText.IsEnabled = true;
            connect.IsEnabled = true;
            disconnect.IsEnabled = false;
            autoClickZL.IsEnabled = false;
            autoClickZR.IsEnabled = false;
            toggleSrint.IsEnabled = false;
            showArmor.IsEnabled = false;
            showHitbox.IsEnabled = false;
            hitboxSize.IsEnabled = false;
            macro.IsEnabled = false;
        }
        #endregion

        #region Get Nintendo Network
        private void GetNintendoNetwork()
        {
            char letter1 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x4E);//[0x10AD1C58] + 0x4E
            char letter2 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x50);
            char letter3 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x52);
            char letter4 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x54);
            char letter5 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x56);
            char letter6 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x58);
            char letter7 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x5A);
            char letter8 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x5C);
            char letter9 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x5E);
            char letter10 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x60);
            char letter11 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x62);
            char letter12 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x64);
            char letter13 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x66);
            char letter14 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x68);
            char letter15 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x6A);
            char letter16 = (char)GeckoU.PeekUInt(GeckoU.PeekUInt(0x10AD1C58) + 0x6C);
            nintendoNetwork = $"{letter1}{letter2}{letter3}{letter4}{letter5}{letter6}{letter7}{letter8}{letter9}{letter10}{letter11}{letter12}{letter13}{letter14}{letter15}{letter16}";
        }
        #endregion

        #endregion

        #region AutoClick
        private void timeBetweenClickZL_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            labelTimeBetweenClickZL.Text = TimeBetweenClick + timeBetweenClickZL.Value * 0.05 + " s";
        }

        private void timeBetweenClickZR_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            labelTimeBetweenClickZR.Text = TimeBetweenClick + timeBetweenClickZR.Value * 0.05 + " s";
        }

        private void frontOfAnEntityZL_Toggled(object sender, ToggledEventArgs e)
        { }

        private void frontOfAnEntityZR_Toggled(object sender, ToggledEventArgs e)
        { }

        private void autoClickZL_Clicked(object sender, EventArgs e)
        {
            Thread autoClickZLThread = new Thread(executeAutoClickZL);
            if (autoClickZL.Text == "OFF")
            {
                autoClickZL.Text = "ON";
                autoClickZL.TextColor = Color.Green;
                autoClickZR.IsEnabled = false;
                autoClickZLThread.Start();
            }
            else
            {
                autoClickZLThread.Abort();
                autoClickZL.Text = "OFF";
                autoClickZL.TextColor = Color.Red;
                autoClickZR.IsEnabled = true;
            }
        }

        private void executeAutoClickZL()
        {
            uint addresss = GeckoU.PeekUInt(0x10A90E6C) + 0x18;
            while (autoClickZL.Text == "ON")
            {
                if (frontOfAnEntityZL.IsToggled)
                {
                    if (GeckoU.PeekUInt(GeckoU.PeekUInt(0x109CD8E4) + 0xC8) != 0x0)
                    {
                        GeckoU.WriteUInt(addresss, 0x00800000);
                        //Old :
                        //GeckoU.CallFunction(0x03C00000, new uint[] { 0x0 });
                    }
                }
                else
                {
                    GeckoU.WriteUInt(addresss, 0x00800000);
                    //Old :
                    //GeckoU.CallFunction(0x03C00000, new uint[] { 0x0 });
                }
                Thread.Sleep((int)timeBetweenClickZL.Value * 50);

            }
        }

        private void autoClickZR_Clicked(object sender, EventArgs e)
        {
            Thread autoClickZRThread = new Thread(executeAutoClickZR);
            if (autoClickZR.Text == "OFF")
            {
                autoClickZR.Text = "ON";
                autoClickZR.TextColor = Color.Green;
                autoClickZL.IsEnabled = false;
                autoClickZRThread.Start();
            }
            else
            {
                autoClickZRThread.Abort();
                autoClickZR.Text = "OFF";
                autoClickZR.TextColor = Color.Red;
                autoClickZL.IsEnabled = true;
            }
        }

        private void executeAutoClickZR()
        {
            //uint addresss = GeckoU.PeekUInt(0x10A90E6C) + 0x18;
            uint addresss = GeckoU.PeekUInt(GeckoU.PeekUInt(0x10A0A624) + 0x9C)+ 0x910;
            while (autoClickZR.Text == "ON")
            {
                if (frontOfAnEntityZR.IsToggled)
                {
                    if (GeckoU.PeekUInt(GeckoU.PeekUInt(0x109CD8E4) + 0xC8) != 0x0)
                    {
                        GeckoU.WriteUInt(addresss, 0x00000004);
                    }
                }
                else
                {
                    GeckoU.WriteUInt(addresss, 0x00000004);
                }
                Thread.Sleep((int)timeBetweenClickZR.Value * 50);
            }
        }
        #endregion

        #region Other
        private void toggleSrint_Toggled(object sender, ToggledEventArgs e)
        {
            if (toggleSrint.IsToggled)
            {
                GeckoU.WriteUInt(0x023405E4, 0x3BC00001);
                GeckoU.WriteUInt(0x031E50E4, 0x4E800020);
            }
            else
            {
                GeckoU.WriteUInt(0x023405E4, 0x38800003);
                GeckoU.WriteUInt(0x031E50E4, 0x800308C8);
            }
        }

        private void showArmor_Toggled(object sender, ToggledEventArgs e)
        {
            if (showArmor.IsToggled)
            {
                GeckoU.WriteUInt(0x02E9B1B0, 0x38800001);
            }
            else
            {
                GeckoU.WriteUInt(0x02E9B1B0, 0x7FC4F378);
            }
        }

        private void showHitbox_Toggled(object sender, ToggledEventArgs e)
        {
            if (showHitbox.IsToggled)
            {
                GeckoU.WriteUInt(0x030FA014, 0x2C090001);

                GeckoU.WriteUInt(0x0384D344, 0x3FC01000);
                GeckoU.WriteUInt(0x0384D348, 0x83DE0000);
                GeckoU.WriteUInt(0x0384D34C, 0x4B9758A8);

                GeckoU.WriteUInt(0x0384D354, 0x3C801000);
                GeckoU.WriteUInt(0x0384D358, 0x80840000);
                GeckoU.WriteUInt(0x0384D35C, 0x4B8AC8F8);

                GeckoU.WriteUInt(0x030F9C50, 0x48753704);
                GeckoU.WriteUInt(0x031C2BF0, 0x4868A754);
            }
            else
            {
                GeckoU.WriteUInt(0x030F9C50, 0x388000FF);
                GeckoU.WriteUInt(0x031C2BF0, 0x7C9E2378);

                GeckoU.WriteUInt(0x030FA014, 0x2C090000);
            }
        }

        private void hitboxSize_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (hitboxSize.Value == 0)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3E800000);
            }
            if (hitboxSize.Value > 0)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3F0A0000);
            }
            if (hitboxSize.Value >= 1)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3F100000);
            }
            if (hitboxSize.Value >= 2)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3F200000);
            }
            if (hitboxSize.Value >= 3)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3F300000);
            }
            if (hitboxSize.Value >= 4)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3F400000);
            }
            if (hitboxSize.Value >= 5)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3F500000);
            }
            if (hitboxSize.Value >= 6)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3F600000);
            }
            if (hitboxSize.Value >= 7)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3F700000);
            }
            if (hitboxSize.Value >= 8)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3F800000);
            }
            if (hitboxSize.Value >= 9)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3FA00000);
            }
            if (hitboxSize.Value >= 10)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3FC00000);
            }
            if (hitboxSize.Value >= 11)
            {
                GeckoU.WriteUInt(0x105DD948, 0x3FE00000);
            }
            if (hitboxSize.Value >= 12)
            {
                GeckoU.WriteUInt(0x105DD948, 0x40000000);
            }
            if (hitboxSize.Value >= 13)
            {
                GeckoU.WriteUInt(0x105DD948, 0x40200000);
            }
            if (hitboxSize.Value >= 14)
            {
                GeckoU.WriteUInt(0x105DD948, 0x40400000);
            }
            if (hitboxSize.Value >= 15)
            {
                GeckoU.WriteUInt(0x105DD948, 0x40600000);
            }
            if (hitboxSize.Value >= 16)
            {
                GeckoU.WriteUInt(0x105DD948, 0x41800000);
            }
            if (hitboxSize.Value >= 17)
            {
                GeckoU.WriteUInt(0x105DD948, 0x41A00000);
            }
            if (hitboxSize.Value >= 18)
            {
                GeckoU.WriteUInt(0x105DD948, 0x41C00000);
            }
            if (hitboxSize.Value >= 19)
            {
                GeckoU.WriteUInt(0x105DD948, 0x41E00000);
            }
            if (hitboxSize.Value == 20)
            {
                GeckoU.WriteUInt(0x105DD948, 0x42000000);
            }
        }

        private void macro_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (macro.Value == 0)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3E800000);
            }
            if (macro.Value > 0)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3E7A0000);
            }
            if (macro.Value > 0)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3E700000);
            }
            if (macro.Value > 0)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3E6A0000);
            }
            if (macro.Value >= 3)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3E600000);
            }
            if (macro.Value >= 4)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3E500000);
            }
            if (macro.Value >= 5)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3E40000);
            }
            if (macro.Value >= 7)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3E300000);
            }
            if (macro.Value >= 6)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3E200000);
            }
            if (macro.Value >= 7)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3E100000);
            }
            if (macro.Value >= 8)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3E000000);
            }
            if (macro.Value >= 9)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3DF00000);
            }
            if (macro.Value >= 10)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3DE00000);
            }
            if (macro.Value >= 11)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3DD00000);
            }
            if (macro.Value >= 12)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3DC00000);
            }
            if (macro.Value >= 13)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3DB00000);
            }
            if (macro.Value >= 14)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3DA00000);
            }
            if (macro.Value >= 15)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3D900000);
            }
            if (macro.Value >= 16)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3D800000);
            }
            if (macro.Value >= 17)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3D700000);
            }
            if (macro.Value >= 18)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3D600000);
            }
            if (macro.Value >= 19)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3D500000);
            }
            if (macro.Value == 20)
            {
                GeckoU.WriteUInt(0x108E0C8C, 0x3D400000);
            }
        }
        #endregion
    }
}


//20191112 minesweaper mini game

//重點在宣告一個該類物件為觸發者 Label xxx = sender as Label; Button xxx = sender as Button; 等等，已取得其資訊。


private void clkLblMine(object sender, EventArgs e)
        {
            Label sdlb = sender as Label;
            switch (sdlb.Name)
            {
                case "lblMine00":
                    if (MineSweaperAct.blnBomb00 == true)
                        lblMine00.Text = "BOMB!!";
                    else
                        lblMine00.Text = Area.area[0,0].ToString();
                    break;
                case "lblMine01":
                    if (MineSweaperAct.blnBomb01 == true)
                        lblMine01.Text = "BOMB!!";
                    else
                        lblMine01.Text = Area.area[0,1].ToString();
                    break;
                case "lblMine02":
                    if (MineSweaperAct.blnBomb02 == true)
                        lblMine02.Text = "BOMB!!";
                    else
                        lblMine02.Text = Area.area[0,2].ToString();
                    break;
                case "lblMine03":
                    if (MineSweaperAct.blnBomb03 == true)
                        lblMine03.Text = "BOMB!!";
                    else
                        lblMine03.Text = Area.area[0,3].ToString();
                    break;
                case "lblMine10":
                    if (MineSweaperAct.blnBomb10 == true)
                        lblMine10.Text = "BOMB!!";
                    else
                        lblMine10.Text = Area.area[1,0].ToString();
                    break;
                case "lblMine11":
                    if (MineSweaperAct.blnBomb11 == true)
                        lblMine11.Text = "BOMB!!";
                    else
                        lblMine11.Text = Area.area[1,1].ToString();
                    break;
                case "lblMine12":
                    if (MineSweaperAct.blnBomb12 == true)
                        lblMine12.Text = "BOMB!!";
                    else
                        lblMine12.Text = Area.area[1,2].ToString();
                    break;
                case "lblMine13":
                    if (MineSweaperAct.blnBomb13 == true)
                        lblMine13.Text = "BOMB!!";
                    else
                        lblMine13.Text = Area.area[1,3].ToString();
                    break;
                case "lblMine20":
                    if (MineSweaperAct.blnBomb20 == true)
                        lblMine20.Text = "BOMB!!";
                    else
                        lblMine20.Text = Area.area[2,0].ToString();
                    break;
                case "lblMine21":
                    if (MineSweaperAct.blnBomb21 == true)
                        lblMine21.Text = "BOMB!!";
                    else
                        lblMine21.Text = Area.area[2,1].ToString();
                    break;
                case "lblMine22":
                    if (MineSweaperAct.blnBomb22 == true)
                        lblMine22.Text = "BOMB!!";
                    else
                        lblMine22.Text = Area.area[2,2].ToString();
                    break;
                case "lblMine23":
                    if (MineSweaperAct.blnBomb23 == true)
                        lblMine23.Text = "BOMB!!";
                    else
                        lblMine23.Text = Area.area[2,3].ToString();
                    break;
                case "lblMine30":
                    if (MineSweaperAct.blnBomb30 == true)
                        lblMine30.Text = "BOMB!!";
                    else
                        lblMine30.Text = Area.area[3,0].ToString();
                    break;
                case "lblMine31":
                    if (MineSweaperAct.blnBomb31 == true)
                        lblMine31.Text = "BOMB!!";
                    else
                        lblMine31.Text = Area.area[3,1].ToString();
                    break;
                case "lblMine32":
                    if (MineSweaperAct.blnBomb32 == true)
                        lblMine32.Text = "BOMB!!";
                    else
                        lblMine32.Text = Area.area[3,2].ToString();
                    break;
                case "lblMine33":
                    if (MineSweaperAct.blnBomb33 == true)
                        lblMine33.Text = "BOMB!!";
                    else
                        lblMine33.Text = Area.area[3,3].ToString();
                    break;
                default:
                    MessageBox.Show("ERROR","ERROR");
                    break;
            }
        }
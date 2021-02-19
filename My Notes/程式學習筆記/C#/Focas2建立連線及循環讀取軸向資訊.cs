//需要fwlib32.cs


private void btnConnect_Click(object sender, EventArgs e)
        {
            //建立連線
            //ushort cncHandle;
            ipAddress = tbxIPAddress.Text;
            portNum = (ushort)Convert.ToInt32(tbxPortNumber.Text);
            timeOut = Convert.ToInt32(tbxTimeOut.Text);

            short ret = Focas1.cnc_allclibhndl3(ipAddress, portNum, timeOut, out cncHandle);

            switch (ret)
            {
                case 0:
                    lblConnectStatus.Text = "Connect";
                    lblConnectStatus.ForeColor = Color.Green;
                    lblCncHandle.Text = cncHandle.ToString();
                    break;

                default:
                    lblConnectStatus.Text = "Disconnect";
                    lblConnectStatus.ForeColor = Color.Red;
                    lblCncHandle.Text = "None";
                    break;
            }

            //抓取座標
            tmrGetCoord.Start();
            /*
            short numOfAxis = -1;
            short coordDataLength = 20;
            Focas1.ODBAXIS absAxes = new Focas1.ODBAXIS();
            Focas1.ODBAXIS relAxes = new Focas1.ODBAXIS();
            Focas1.ODBAXIS macAxes = new Focas1.ODBAXIS();
            System.Timers.Timer tgetLocation = new System.Timers.Timer(1000)
            {
                AutoReset = true,
                Enabled = true
            };

            tgetLocation.Elapsed += new ElapsedEventHandler(getLocation);
            
            void getLocation (object source, ElapsedEventArgs eve)
            {
                Focas1.cnc_absolute(cncHandle,numOfAxis,coordDataLength,absAxes);
                lblAbsX.Text = absAxes.data[0].ToString();
            }
            */

        }

        private void tmrGetCoordfn(object sender, EventArgs e)
        {
            short numOfAxis = -1;
            short coordDataLength = 20;
            Focas1.ODBAXIS absAxes = new Focas1.ODBAXIS();
            Focas1.ODBAXIS relAxes = new Focas1.ODBAXIS();
            Focas1.ODBAXIS macAxes = new Focas1.ODBAXIS();
            getLocation();
            void getLocation()
            {
                Focas1.cnc_absolute(cncHandle, numOfAxis, coordDataLength, absAxes);
                lblAbsX.Text = absAxes.data[0].ToString();
                lblAbsY.Text = absAxes.data[1].ToString();
                lblAbsZ.Text = absAxes.data[2].ToString();
                Focas1.cnc_relative(cncHandle, numOfAxis, coordDataLength, relAxes);
                lblRelX.Text = relAxes.data[0].ToString();
                lblRelY.Text = relAxes.data[1].ToString();
                lblRelZ.Text = relAxes.data[2].ToString();
                Focas1.cnc_machine(cncHandle, numOfAxis, coordDataLength, macAxes);
                lblMacX.Text = macAxes.data[0].ToString();
                lblMacY.Text = macAxes.data[1].ToString();
                lblMacZ.Text = macAxes.data[2].ToString();
            }
        }
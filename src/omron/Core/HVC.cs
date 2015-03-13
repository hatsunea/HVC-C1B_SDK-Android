/*
 * Copyright (C) 2014-2015 HATSUNE, Akira (Original OMRON Corporation)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Threading.Tasks;
using omron.HVC;

namespace omron.HVC
{
    /// <summary>
    /// HVC object<br>
    /// [Description]<br>
    /// New class object definition HVC<br>
    /// </summary>
    public abstract class HVC
    {
        /// <summary>
        /// Execution flag for Human Body Detection<br>
        /// Specify in inExec of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ACTIV_BODY_DETECTION = 0x00000001;
        /// <summary>
        /// Execution flag for Hand Detection<br>
        /// Specify in inExec of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ACTIV_HAND_DETECTION = 0x00000002;
        /// <summary>
        /// Execution flag for Face Detection<br>
        /// Specify in inExec of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ACTIV_FACE_DETECTION = 0x00000004;
        /// <summary>
        /// Execution flag for face direction estimation<br>
        /// Specify in inExec of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ACTIV_FACE_DIRECTION = 0x00000008;
        /// <summary>
        /// Execution flag for Age Estimation<br>
        /// Specify in inExec of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ACTIV_AGE_ESTIMATION = 0x00000010;
        /// <summary>
        /// Execution flag for Gender Estimation<br>
        /// Specify in inExec of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ACTIV_GENDER_ESTIMATION = 0x00000020;
        /// <summary>
        /// Execution flag for Gaze Estimation<br>
        /// Specify in inExec of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ACTIV_GAZE_ESTIMATION = 0x00000040;
        /// <summary>
        /// Execution flag for Blink Estimation<br>
        /// Specify in inExec of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ACTIV_BLINK_ESTIMATION = 0x00000080;
        /// <summary>
        /// Execution flag for Expression Estimation<br>
        /// Specify in inExec of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ACTIV_EXPRESSION_ESTIMATION = 0x00000100;

        /// <summary>
        /// Normal end<br>
        /// Return value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_NORMAL = 0;
        /// <summary>
        /// Parameter error (invalid inExec specification)<br>
        /// Return value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ERROR_PARAMETER = -1;
        /// <summary>
        /// Device error (device not found)<br>
        /// Return value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ERROR_NODEVICES = -2;
        /// <summary>
        /// Connection error (cannot connect to HVC device)<br>
        /// return value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ERROR_DISCONNECTED = -3;
        /// <summary>
        /// Input error (cannot re-input as HVC is already executing)<br>
        /// Return value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ERROR_BUSY = -4;
        /// <summary>
        /// Send signal timeout error (timeout while sending command signal to HVC)<br>
        /// Return value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ERROR_SEND_DATA = -10;
        /// <summary>
        /// Receive header signal timeout error (timeout while receiving header signal from HVC)<br>
        /// Return value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ERROR_HEADER_TIMEOUT = -20;
        /// <summary>
        /// Invalid header error (invalid header data received from HVC)<br>
        /// Return value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ERROR_HEADER_INVALID = -21;
        /// <summary>
        /// Receive data signal timeout error (timeout while receiving data signal from HVC)<br>
        /// Return value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_ERROR_DATA_TIMEOUT = -22;

        /// <summary>
        /// Normal end<br>
        /// outStatus value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_STATUS_NORMAL = 0;
        /// <summary>
        /// Unknown command<br>
        /// outStatus value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_STATUS_UNKNOWN = -1;
        /// <summary>
        /// Unexpected error<br>
        /// outStatus value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_STATUS_VARIOUS = -2;
        /// <summary>
        /// Invalid command<br>
        /// outStatus value of Execute(int inExec, byte[] outStatus)<br>
        /// </summary>
        public const int HVC_STATUS_INVALID = -3;

        /// <summary>
        /// Male<br>
        /// Gender Estimation result value<br>
        /// </summary>
        public const int HVC_GEN_MALE = 1;
        /// <summary>
        /// Female<br>
        /// Gender Estimation result value<br>
        /// </summary>
        public const int HVC_GEN_FEMALE = 0;

        /// <summary>
        /// Neutral<br>
        /// Expression Estimation result value<br>
        /// </summary>
        public const int HVC_EX_NEUTRAL = 1;
        /// <summary>
        /// Happiness<br>
        /// Expression Estimation result value<br>
        /// </summary>
        public const int HVC_EX_HAPPINESS = 2;
        /// <summary>
        /// Surprise<br>
        /// Expression Estimation result value<br>
        /// </summary>
        public const int HVC_EX_SURPRISE = 3;
        /// <summary>
        /// Anger<br>
        /// Expression Estimation result value<br>
        /// </summary>
        public const int HVC_EX_ANGER = 4;
        /// <summary>
        /// Sadness<br>
        /// Expression Estimation result value<br>
        /// </summary>
        public const int HVC_EX_SADNESS = 5;

        /// <summary>
        /// HVC constructor<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br>
        /// </summary>
        protected internal HVC()
        {
        }

        protected internal void Dispose()
        {
        }

        public abstract bool IsBusy();
        public abstract Task<int> Execute(int inExec, HVC_RES res);
        public abstract Task<int> SetParam(HVC_PRM prm);
        public abstract Task<int> GetParam(HVC_PRM prm);
        public abstract Task<int> GetVersion(HVC_VER ver);

        /* Command number */
        private const char HVC_COM_GET_VERSION = (char)0x00;
        private const char HVC_COM_SET_CAMERA_ANGLE = (char)0x01;
        private const char HVC_COM_GET_CAMERA_ANGLE = (char)0x02;
        private const char HVC_COM_EXECUTE = (char)0x03;
        private const char HVC_COM_SET_THRESHOLD = (char)0x05;
        private const char HVC_COM_GET_THRESHOLD = (char)0x06;
        private const char HVC_COM_SET_SIZE_RANGE = (char)0x07;
        private const char HVC_COM_GET_SIZE_RANGE = (char)0x08;
        private const char HVC_COM_SET_DETECTION_ANGLE = (char)0x09;
        private const char HVC_COM_GET_DETECTION_ANGLE = (char)0x0A;

        /* Header for send signal data  */
        private const int SEND_HEAD_SYNCBYTE = 0;
        private const int SEND_HEAD_COMMANDNO = 1;
        private const int SEND_HEAD_DATALENGTHLSB = 2;
        private const int SEND_HEAD_DATALENGTHMSB = 3;
        private const int SEND_HEAD_NUM = 4;

        /* Header for receive signal data */
        private const int RECEIVE_HEAD_SYNCBYTE = 0;
        private const int RECEIVE_HEAD_STATUS = 1;
        private const int RECEIVE_HEAD_DATALENLL = 2;
        private const int RECEIVE_HEAD_DATALENLM = 3;
        private const int RECEIVE_HEAD_DATALENML = 4;
        private const int RECEIVE_HEAD_DATALENMM = 5;
        private const int RECEIVE_HEAD_NUM = 6;

        protected internal abstract int Send(byte[] inData);
        protected internal abstract int Receive(int inTimeOutTime, int inDataSize, byte[] outResult);

        /// <summary>
        /// Send command signal<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inCommandNo"> command number<br> </param>
        /// <param name="inDataSize"> sending signal data size<br> </param>
        /// <param name="inData"> sending signal data<br> </param>
        public virtual int SendCommand(char inCommandNo, int inDataSize, byte[] inData)
        {
            int nRet = 0;
            byte[] sendData = new byte[SEND_HEAD_NUM + inDataSize];

            /* Create header */
            sendData[SEND_HEAD_SYNCBYTE] = 0xFE;
            sendData[SEND_HEAD_COMMANDNO] = (byte)inCommandNo;
            sendData[SEND_HEAD_DATALENGTHLSB] = unchecked((byte)(inDataSize & 0xff));
            sendData[SEND_HEAD_DATALENGTHMSB] = unchecked((byte)((inDataSize >> 8) & 0xff));

            for (int i = 0; i < inDataSize; i++)
            {
                sendData[SEND_HEAD_NUM + i] = unchecked((byte)(inData[i] & 0xff));
            }

            /* Send command signal */
            nRet = Send(sendData);
            if (nRet != SEND_HEAD_NUM + inDataSize)
            {
                return HVC_ERROR_SEND_DATA;
            }
            return HVC_NORMAL;
        }

        /// <summary>
        /// Receive header<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="outDataSize"> receive signal data length<br> </param>
        /// <param name="outStatus"> status<br> </param>
        public virtual int ReceiveHeader(int inTimeOutTime, int[] outDataSize, byte[] outStatus)
        {
            int nRet = 0;
            byte[] headerData;

            headerData = new byte[32];

            /* Get header part */
            nRet = Receive(inTimeOutTime, RECEIVE_HEAD_NUM, headerData);
            if (nRet != RECEIVE_HEAD_NUM)
            {
                return HVC_ERROR_HEADER_TIMEOUT;
            }
            else if ((headerData[RECEIVE_HEAD_SYNCBYTE] & 0xff) != 0xfe)
            {
                /* Different value indicates an invalid result */
                return HVC_ERROR_HEADER_INVALID;
            }

            /* Get data length */
            outDataSize[0] = (headerData[RECEIVE_HEAD_DATALENLL] & 0xff) + ((headerData[RECEIVE_HEAD_DATALENLM] & 0xff) << 8) + ((headerData[RECEIVE_HEAD_DATALENML] & 0xff) << 16) + ((headerData[RECEIVE_HEAD_DATALENMM] & 0xff) << 24);

            /* Get command execution result */
            outStatus[0] = headerData[RECEIVE_HEAD_STATUS];
            return HVC_NORMAL;
        }

        /// <summary>
        /// Receive data<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="inDataSize"> receive signal data size<br> </param>
        /// <param name="outResult"> receive signal data<br> </param>
        public virtual int ReceiveData(int inTimeOutTime, int inDataSize, byte[] outResult)
        {
            int nRet = 0;

            if (inDataSize <= 0)
            {
                return HVC_NORMAL;
            }

            /* Receive data */
            nRet = Receive(inTimeOutTime, inDataSize, outResult);
            if (nRet != inDataSize)
            {
                return HVC_ERROR_DATA_TIMEOUT;
            }
            return HVC_NORMAL;
        }

        /// <summary>
        /// GetVersion<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="outStatus"> response code<br> </param>
        public virtual int GetVersion(int inTimeOutTime, byte[] outStatus, HVC_VER ver)
        {
            int nRet = 0;
            int[] nSize = { };
            byte[] sendData;
            byte[] recvData;

            sendData = new byte[32];
            recvData = new byte[32];

            /* Send GetVersion command signal*/
            nRet = SendCommand(HVC_COM_GET_VERSION, 0, sendData);
            if (nRet != 0)
            {
                return nRet;
            }

            /* Receive header */
            nRet = ReceiveHeader(inTimeOutTime, nSize, outStatus);
            if (nRet != 0)
            {
                return nRet;
            }

            if (nSize[0] > ver.GetSize())
            {
                nSize[0] = ver.GetSize();
            }

            /* Receive data */
            nRet = ReceiveData(inTimeOutTime, nSize[0], recvData);
            for (int i = 0; i < ver.Version.Length; i++)
            {
                ver.Version[i] = (char)recvData[i];
            }
            ver.Major = recvData[ver.Version.Length];
            ver.Minor = recvData[ver.Version.Length + 1];
            ver.Relese = recvData[ver.Version.Length + 2];
            ver.Rev = recvData[ver.Version.Length + 3] + (recvData[ver.Version.Length + 4] << 8) + (recvData[ver.Version.Length + 5] << 16) + (recvData[ver.Version.Length + 6] << 24);
            return nRet;
        }

        /// <summary>
        /// SetCameraAngle<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="outStatus"> response code<br> </param>
        public virtual int SetCameraAngle(int inTimeOutTime, byte[] outStatus, HVC_PRM param)
        {
            int nRet = 0;
            int[] nSize = { };
            byte[] sendData;

            sendData = new byte[32];

            sendData[0] = unchecked((byte)(param.CameraAngle & 0xff));
            /* Send SetCameraAngle command signal */
            nRet = SendCommand(HVC_COM_SET_CAMERA_ANGLE, 1, sendData);
            if (nRet != 0)
            {
                return nRet;
            }

            /* Receive header */
            nRet = ReceiveHeader(inTimeOutTime, nSize, outStatus);
            if (nRet != 0)
            {
                return nRet;
            }
            return HVC_NORMAL;
        }

        /// <summary>
        /// GetCameraAngle<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="outStatus"> response code<br> </param>
        public virtual int GetCameraAngle(int inTimeOutTime, byte[] outStatus, HVC_PRM param)
        {
            int nRet = 0;
            int[] nSize = { };
            byte[] sendData;
            byte[] recvData;

            sendData = new byte[32];
            recvData = new byte[32];

            /* Send GetCameraAngle command signal */
            nRet = SendCommand(HVC_COM_GET_CAMERA_ANGLE, 0, sendData);
            if (nRet != 0)
            {
                return nRet;
            }

            /* Receive header */
            nRet = ReceiveHeader(inTimeOutTime, nSize, outStatus);
            if (nRet != 0)
            {
                return nRet;
            }

            if (nSize[0] > 1)
            {
                nSize[0] = 1;
            }

            /* Receive data */
            nRet = ReceiveData(inTimeOutTime, nSize[0], recvData);
            param.CameraAngle = recvData[0];
            return nRet;
        }

        /// <summary>
        /// SetThreshold<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="outStatus"> response code<br> </param>
        public virtual int SetThreshold(int inTimeOutTime, byte[] outStatus, HVC_PRM param)
        {
            int nRet = 0;
            int[] nSize = { };
            byte[] sendData;

            sendData = new byte[32];

            sendData[0] = unchecked((byte)(param.Body.Threshold & 0xff));
            sendData[1] = unchecked((byte)((param.Body.Threshold >> 8) & 0xff));
            sendData[2] = unchecked((byte)(param.Hand.Threshold & 0xff));
            sendData[3] = unchecked((byte)((param.Hand.Threshold >> 8) & 0xff));
            sendData[4] = unchecked((byte)(param.Face.Threshold & 0xff));
            sendData[5] = unchecked((byte)((param.Face.Threshold >> 8) & 0xff));
            sendData[6] = 0;
            sendData[7] = 0;
            /* Send SetThreshold command signal */
            nRet = SendCommand(HVC_COM_SET_THRESHOLD, 8, sendData);
            if (nRet != 0)
            {
                return nRet;
            }

            /* Receive header */
            nRet = ReceiveHeader(inTimeOutTime, nSize, outStatus);
            if (nRet != 0)
            {
                return nRet;
            }
            return HVC_NORMAL;
        }

        /// <summary>
        /// GetThreshold<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="outStatus"> response code<br> </param>
        public virtual int GetThreshold(int inTimeOutTime, byte[] outStatus, HVC_PRM param)
        {
            int nRet = 0;
            int[] nSize = { };
            byte[] sendData;
            byte[] recvData;

            sendData = new byte[32];
            recvData = new byte[32];

            /* Send GetThreshold command signal */
            nRet = SendCommand(HVC_COM_GET_THRESHOLD, 0, sendData);
            if (nRet != 0)
            {
                return nRet;
            }

            /* Receive header */
            nRet = ReceiveHeader(inTimeOutTime, nSize, outStatus);
            if (nRet != 0)
            {
                return nRet;
            }

            if (nSize[0] > 8)
            {
                nSize[0] = 8;
            }

            /* Receive data */
            nRet = ReceiveData(inTimeOutTime, nSize[0], recvData);
            param.Body.Threshold = recvData[0] + (recvData[1] << 8);
            param.Hand.Threshold = recvData[2] + (recvData[3] << 8);
            param.Face.Threshold = recvData[4] + (recvData[5] << 8);
            return nRet;
        }

        /// <summary>
        /// SetSizeRange<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="outStatus"> response code<br> </param>
        public virtual int SetSizeRange(int inTimeOutTime, byte[] outStatus, HVC_PRM param)
        {
            int nRet = 0;
            int[] nSize = { };
            byte[] sendData;

            sendData = new byte[32];

            sendData[0] = unchecked((byte)(param.Body.MinSize & 0xff));
            sendData[1] = unchecked((byte)((param.Body.MinSize >> 8) & 0xff));
            sendData[2] = unchecked((byte)(param.Body.MaxSize & 0xff));
            sendData[3] = unchecked((byte)((param.Body.MaxSize >> 8) & 0xff));
            sendData[4] = unchecked((byte)(param.Hand.MinSize & 0xff));
            sendData[5] = unchecked((byte)((param.Hand.MinSize >> 8) & 0xff));
            sendData[6] = unchecked((byte)(param.Hand.MaxSize & 0xff));
            sendData[7] = unchecked((byte)((param.Hand.MaxSize >> 8) & 0xff));
            sendData[8] = unchecked((byte)(param.Face.MinSize & 0xff));
            sendData[9] = unchecked((byte)((param.Face.MinSize >> 8) & 0xff));
            sendData[10] = unchecked((byte)(param.Face.MaxSize & 0xff));
            sendData[11] = unchecked((byte)((param.Face.MaxSize >> 8) & 0xff));
            /* Send SetSizeRange command signal */
            nRet = SendCommand(HVC_COM_SET_SIZE_RANGE, 12, sendData);
            if (nRet != 0)
            {
                return nRet;
            }

            /* Receive header */
            nRet = ReceiveHeader(inTimeOutTime, nSize, outStatus);
            if (nRet != 0)
            {
                return nRet;
            }
            return HVC_NORMAL;
        }

        /// <summary>
        /// GetSizeRange<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="outStatus"> response code<br> </param>
        public virtual int GetSizeRange(int inTimeOutTime, byte[] outStatus, HVC_PRM param)
        {
            int nRet = 0;
            int[] nSize = { };
            byte[] sendData;
            byte[] recvData;

            sendData = new byte[32];
            recvData = new byte[32];

            /* Send GetSizeRange command signal */
            nRet = SendCommand(HVC_COM_GET_SIZE_RANGE, 0, sendData);
            if (nRet != 0)
            {
                return nRet;
            }

            /* Receive header */
            nRet = ReceiveHeader(inTimeOutTime, nSize, outStatus);
            if (nRet != 0)
            {
                return nRet;
            }

            if (nSize[0] > 12)
            {
                nSize[0] = 12;
            }

            /* Receive data */
            nRet = ReceiveData(inTimeOutTime, nSize[0], recvData);
            param.Body.MinSize = recvData[0] + (recvData[1] << 8);
            param.Body.MaxSize = recvData[2] + (recvData[3] << 8);
            param.Hand.MinSize = recvData[4] + (recvData[5] << 8);
            param.Hand.MaxSize = recvData[6] + (recvData[7] << 8);
            param.Face.MinSize = recvData[8] + (recvData[9] << 8);
            param.Face.MaxSize = recvData[10] + (recvData[11] << 8);
            return nRet;
        }

        /// <summary>
        /// SetFaceDetectionAngle<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="outStatus"> response code<br> </param>
        public virtual int SetFaceDetectionAngle(int inTimeOutTime, byte[] outStatus, HVC_PRM param)
        {
            int nRet = 0;
            int[] nSize = { };
            byte[] sendData;

            sendData = new byte[32];

            sendData[0] = unchecked((byte)(param.Face.Pose & 0xff));
            sendData[1] = unchecked((byte)(param.Face.Angle & 0xff));
            /* Send SetFaceDetectionAngle command signal */
            nRet = SendCommand(HVC_COM_SET_DETECTION_ANGLE, 2, sendData);
            if (nRet != 0)
            {
                return nRet;
            }

            /* Receive header */
            nRet = ReceiveHeader(inTimeOutTime, nSize, outStatus);
            if (nRet != 0)
            {
                return nRet;
            }
            return HVC_NORMAL;
        }

        /// <summary>
        /// GetFaceDetectionAngle<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="outStatus"> response code<br> </param>
        public virtual int GetFaceDetectionAngle(int inTimeOutTime, byte[] outStatus, HVC_PRM param)
        {
            int nRet = 0;
            int[] nSize = { };
            byte[] sendData;
            byte[] recvData;

            sendData = new byte[32];
            recvData = new byte[32];

            /* Send GetFaceDetectionAngle signal command */
            nRet = SendCommand(HVC_COM_GET_DETECTION_ANGLE, 0, sendData);
            if (nRet != 0)
            {
                return nRet;
            }

            /* Receive header */
            nRet = ReceiveHeader(inTimeOutTime, nSize, outStatus);
            if (nRet != 0)
            {
                return nRet;
            }

            if (nSize[0] > 2)
            {
                nSize[0] = 2;
            }

            /* Receive data */
            nRet = ReceiveData(inTimeOutTime, nSize[0], recvData);
            param.Face.Pose = recvData[0];
            param.Face.Angle = recvData[1];
            return nRet;
        }

        /// <summary>
        /// Execute<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inTimeOutTime"> timeout time<br> </param>
        /// <param name="inExec"> executable function<br> </param>
        /// <param name="outStatus"> response code<br> </param>
        public virtual int Execute(int inTimeOutTime, int inExec, byte[] outStatus, HVC_RES result)
        {
            int nRet = 0;
            int[] nSize = { 0 };
            byte[] sendData;
            byte[] recvData;

            sendData = new byte[32];
            recvData = new byte[32];

            result.ExecutedFunc = inExec;
            result.Body.Clear();
            result.Hand.Clear();
            result.Face.Clear();

            /* Send Execute command signal */
            sendData[0] = unchecked((byte)(inExec & 0xff));
            sendData[1] = unchecked((byte)((inExec >> 8) & 0xff));
            sendData[2] = 0;
            nRet = SendCommand(HVC_COM_EXECUTE, 3, sendData);
            if (nRet != 0)
            {
                return nRet;
            }

            /* Receive header */
            nRet = ReceiveHeader(inTimeOutTime, nSize, outStatus);
            if (nRet != 0)
            {
                return nRet;
            }

            int numBody = 0;
            int numHand = 0;
            int numFace = 0;
            /* Receive result data */
            if (nSize[0] >= 4)
            {
                nRet = ReceiveData(inTimeOutTime, 4, recvData);
                numBody = (recvData[0] & 0xff);
                numHand = (recvData[1] & 0xff);
                numFace = (recvData[2] & 0xff);
                if (nRet != 0)
                {
                    return nRet;
                }
                nSize[0] -= 4;
            }

            /* Get Human Body Detection result */
            for (int i = 0; i < numBody; i++)
            {
                var body = new omron.HVC.HVC_RES.DetectionResult((new HVC_RES()));
                 
                if (nSize[0] >= 8)
                {
                    nRet = ReceiveData(inTimeOutTime, 8, recvData);
                    body.PosX = ((recvData[0] & 0xff) + (recvData[1] << 8));
                    body.PosY = ((recvData[2] & 0xff) + (recvData[3] << 8));
                    body.Size = ((recvData[4] & 0xff) + (recvData[5] << 8));
                    body.Confidence = ((recvData[6] & 0xff) + (recvData[7] << 8));
                    if (nRet != 0)
                    {
                        return nRet;
                    }
                    nSize[0] -= 8;
                }

                result.Body.Add(body);
            }

            /* Get Hand Detection result */
            for (int i = 0; i < numHand; i++)
            {
                var hand = new omron.HVC.HVC_RES.DetectionResult((new HVC_RES()));

                if (nSize[0] >= 8)
                {
                    nRet = ReceiveData(inTimeOutTime, 8, recvData);
                    hand.PosX = ((recvData[0] & 0xff) + (recvData[1] << 8));
                    hand.PosY = ((recvData[2] & 0xff) + (recvData[3] << 8));
                    hand.Size = ((recvData[4] & 0xff) + (recvData[5] << 8));
                    hand.Confidence = ((recvData[6] & 0xff) + (recvData[7] << 8));
                    if (nRet != 0)
                    {
                        return nRet;
                    }
                    nSize[0] -= 8;
                }

                result.Hand.Add(hand);
            }

            /* Face-related results */
            for (int i = 0; i < numFace; i++)
            {
                var face = new omron.HVC.HVC_RES.FaceResult((new HVC_RES()));

                /* Face Detection result */
                if (0 != (result.ExecutedFunc & HVC_ACTIV_FACE_DETECTION))
                {
                    if (nSize[0] >= 8)
                    {
                        nRet = ReceiveData(inTimeOutTime, 8, recvData);
                        face.PosX = ((recvData[0] & 0xff) + (recvData[1] << 8));
                        face.PosY = ((recvData[2] & 0xff) + (recvData[3] << 8));
                        face.Size = ((recvData[4] & 0xff) + (recvData[5] << 8));
                        face.Confidence = ((recvData[6] & 0xff) + (recvData[7] << 8));
                        if (nRet != 0)
                        {
                            return nRet;
                        }
                        nSize[0] -= 8;
                    }
                }

                /* Face direction */
                if (0 != (result.ExecutedFunc & HVC_ACTIV_FACE_DIRECTION))
                {
                    if (nSize[0] >= 8)
                    {
                        nRet = ReceiveData(inTimeOutTime, 8, recvData);
                        face.Dir.Yaw = (short)((recvData[0] & 0xff) + (recvData[1] << 8));
                        face.Dir.Pitch = (short)((recvData[2] & 0xff) + (recvData[3] << 8));
                        face.Dir.Roll = (short)((recvData[4] & 0xff) + (recvData[5] << 8));
                        face.Dir.Confidence = (short)((recvData[6] & 0xff) + (recvData[7] << 8));
                        if (nRet != 0)
                        {
                            return nRet;
                        }
                        nSize[0] -= 8;
                    }
                }

                /* Age */
                if (0 != (result.ExecutedFunc & HVC_ACTIV_AGE_ESTIMATION))
                {
                    if (nSize[0] >= 3)
                    {
                        nRet = ReceiveData(inTimeOutTime, 3, recvData);
                        face.Age.Age = recvData[0];
                        face.Age.Confidence = (short)((recvData[1] & 0xff) + (recvData[2] << 8));
                        if (nRet != 0)
                        {
                            return nRet;
                        }
                        nSize[0] -= 3;
                    }
                }

                /* Gender */
                if (0 != (result.ExecutedFunc & HVC_ACTIV_GENDER_ESTIMATION))
                {
                    if (nSize[0] >= 3)
                    {
                        nRet = ReceiveData(inTimeOutTime, 3, recvData);
                        face.Gen.Gender = recvData[0];
                        face.Gen.Confidence = (short)((recvData[1] & 0xff) + (recvData[2] << 8));
                        if (nRet != 0)
                        {
                            return nRet;
                        }
                        nSize[0] -= 3;
                    }
                }

                /* Gaze */
                if (0 != (result.ExecutedFunc & HVC_ACTIV_GAZE_ESTIMATION))
                {
                    if (nSize[0] >= 2)
                    {
                        nRet = ReceiveData(inTimeOutTime, 2, recvData);
                        face.Gaze.GazeLR = recvData[0];
                        face.Gaze.GazeUD = recvData[1];
                        if (nRet != 0)
                        {
                            return nRet;
                        }
                        nSize[0] -= 2;
                    }
                }

                /* Blink */
                if (0 != (result.ExecutedFunc & HVC_ACTIV_BLINK_ESTIMATION))
                {
                    if (nSize[0] >= 4)
                    {
                        nRet = ReceiveData(inTimeOutTime, 4, recvData);
                        face.Blink.RatioL = (short)((recvData[0] & 0xff) + (recvData[1] << 8));
                        face.Blink.RatioR = (short)((recvData[2] & 0xff) + (recvData[3] << 8));
                        if (nRet != 0)
                        {
                            return nRet;
                        }
                        nSize[0] -= 4;
                    }
                }

                /* Expression */
                if (0 != (result.ExecutedFunc & HVC_ACTIV_EXPRESSION_ESTIMATION))
                {
                    if (nSize[0] >= 3)
                    {
                        nRet = ReceiveData(inTimeOutTime, 3, recvData);
                        face.Exp.Expression = recvData[0];
                        face.Exp.Score = recvData[1];
                        face.Exp.Degree = recvData[2];
                        if (nRet != 0)
                        {
                            return nRet;
                        }
                        nSize[0] -= 3;
                    }
                }

                result.Face.Add(face);
            }

            return HVC_NORMAL;
        }
    }

}
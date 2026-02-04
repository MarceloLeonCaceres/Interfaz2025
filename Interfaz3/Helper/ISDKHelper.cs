using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilitarios;

namespace SDK
{
    public interface ISDKHelper
    {
        string biometricType { get; }
        SDKHelper.SupportBiometricType supportBiometricType { get; }

        int bio_GetAllUserInfo(DataGridView dgvUserInfo);
        string bio_GetNumeroDeSerie(out string sSN);
        clsIntString bio_LeeMarcaciones(DataTable dt_log, DataGridView dgvUserinfo);
        clsIntString bio_LeeMarcaciones(IProgress<int> barraProgreso, Label etiqueta, DataTable dt_log, DataGridView dgvUserinfo, CancellationToken cancelToken);
        clsIntString bio_LeeMarcaciones_ProgressBar(ProgressBar progressB, DataTable dt_log, DataGridView dgvUserinfo, IProgress<int> progress, CancellationToken cancelToken, int nMarcaciones);
        int bio_TraeFPSeleccionados(List<string> lstEmpleados, DataGridView dgvUsuariosLeidos, out int numHuellas, out int numUsuarios);
        int ConsultaUsuariosReloj(DataTable dtUsersReloj, out int numUsuarios);
        bool GetConnectState();
        int GetMachineNumber();
        Task Run(ProgressBar progressBar, Label label, DataTable data, DataGridView dataGrid);
        void SetConnectState(bool state);
        void SetMachineNumber(int Number);
        int sta_batch_SetAllUserFPInfo(ProgressBar prgSta, List<DataGridViewRow> lvUserInfo);
        int sta_btnPowerOffDevice();
        int sta_btnRestartDevice();
        int sta_ClearAdmin();
        int sta_ClearAllData();
        int sta_ClearAllFps();
        int sta_ClearAllLogs();
        int sta_ClearAllUsers();
        int sta_ConnectTCP(string ip, string port, string commKey);
        int sta_DeleteAttLog();
        int sta_DeleteEnrollData(DataGridView dgvBitacora, ProgressBar prgSta, int iOpcion = 0, List<string> lstEmpleados = null);
        int sta_DeleteEnrollData(ListBox lblOutputInfo, ComboBox cbUseID, ComboBox cbBackupDE);
        int sta_DelUserTmp(ListBox lblOutputInfo, ComboBox cbUseID, ComboBox cbFingerIndex);
        void sta_DisConnect();
        int sta_GetAllUserFaceInfo(DataGridView dgvBitacora, ProgressBar prgSta, DataGridView dgvUsuariosLeidos, int iOpcion = 0, List<string> lstEmpleados = null);
        int sta_GetAllUserFaceInfo(ListBox lblOutputInfo, ProgressBar prgSta, ListView lvUserInfo);
        int sta_GetAllUserFPInfo(DataGridView dgvBitacora, ProgressBar prgSta, DataGridView dgvUsuariosLeidos, DataTable dtEmpleados, int iOpcion = 0, List<string> codigosEmpleados = null);
        int sta_GetAllUserTarjetaInfo(DataGridView dgvBitacora, ProgressBar prgSta, DataGridView dgvUsuariosLeidos, DataTable dtEmpleados, int iOpcion = 0, List<string> codigosEmpleados = null);
        void sta_getBiometricType();
        void sta_getBiometricVersion();
        int sta_GetCapacityInfo(out int adminCnt, out int userCount, out int fpCnt, out int recordCnt, out int pwdCnt, out int oplogCnt, out int faceCnt);
        int sta_GetDeviceInfo(out int idMaquina, out string sFirmver, out string sMac, out string sPlatform, out string sSN, out string sProductTime, out string sDeviceName, out int iFPAlg, out int iFaceAlg, out string sProducter);
        int sta_GetDeviceTime(Label lbDeviceTime, out DateTime fechaHora);
        List<SDKHelper.Employee> sta_getEmployees();
        int sta_readAttLog(DataTable dt_log);
        int sta_SetAllUserFaceInfo(ProgressBar prgSta, List<DataGridViewRow> lvUserInfo);
        int sta_SetAllUserFPInfo(ProgressBar prgSta, List<DataGridViewRow> lvUserInfo);
        int sta_SetDeviceTime(DateTime dtDeviceTime);
        void sta_setEmployees(List<SDKHelper.Employee> employees);
        int sta_SYNCTime();
        int subeHuellasLote(DataTable dt);
    }
}
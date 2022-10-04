#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.Personnel.Enums
{

    public enum MilitaryStatus
    {
        /// <summary>
        /// ���� ����� 
        /// </summary>
        Served,//���� ����� 
        /// <summary>
        /// ����
        /// </summary>
        Exempt,//����
        /// <summary>
        /// ����
        /// </summary>
        Delayed,//����
        /// <summary>
        /// ����
        /// </summary>
        NotObligedToServe,//����
        /// <summary>
        /// ������
        /// </summary>
        Hold,// ������
        /// <summary>
        /// ��� ��� �����
        /// </summary>
        Serving,//��� ��� �����
        /// <summary>
        /// ������
        /// </summary>
        Reserve,//������
        
        /// <summary>
        /// �� ���
        /// </summary>
        Nothing//�� ���
    }
} 
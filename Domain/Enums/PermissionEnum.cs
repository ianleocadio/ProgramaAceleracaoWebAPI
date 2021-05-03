using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum PermissionEnum
    {
        /// <summary>
        /// Cria usuários
        /// </summary>
        [Description("Cria usuários")]
        UserCreate = 1,

        /// <summary>
        /// Atualiza usuários
        /// </summary>
        [Description("Atualiza usuários")]
        UserUpdate = 2,

        /// <summary>
        /// Consulta usuários
        /// </summary>
        [Description("Consulta usuários")]
        UserRead = 3,

        /// <summary>
        /// Remove usuários
        /// </summary>
        [Description("Remove usuários")]
        UserDelete = 4,

        /// <summary>
        /// Administra permissões
        /// </summary>
        [Description("Administra permissões")]
        PermissionManage = 5,
    }
}

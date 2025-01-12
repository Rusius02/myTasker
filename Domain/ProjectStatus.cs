using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public enum ProjectStatus
    {
        NotStarted, // Pas encore commencé
        InProgress, // En cours
        Completed,  // Terminé
        OnHold      // En attente
    }
}

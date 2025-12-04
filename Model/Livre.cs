using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bibliotheque.Model
{
    // Optimisation : Implémentation de INotifyPropertyChanged (vu dans Suite_Chapitre_6.pdf)
    // Cela permet à l'interface utilisateur de se mettre à jour automatiquement
    // si la moyenne ou le nombre d'évaluations change, même dans une liste.
    public class Livre : INotifyPropertyChanged
    {
        // Champs privés pour stocker les valeurs
        private double _moyenneEvaluation;
        private int _nombreEvaluations;

        // Propriétés standard (ne changent pas souvent)
        public string Titre { get; set; } = string.Empty;
        public string Auteur { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string MaisonEdition { get; set; } = string.Empty;
        public string DatePublication { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Propriété avec notification : MoyenneEvaluation
        public double MoyenneEvaluation
        {
            get => _moyenneEvaluation;
            set
            {
                if (_moyenneEvaluation != value)
                {
                    _moyenneEvaluation = value;
                    OnPropertyChanged(); // Notifie la vue du changement
                    OnPropertyChanged(nameof(EstFavori)); // Notifie aussi que le statut favori peut avoir changé
                }
            }
        }

        // Propriété avec notification : NombreEvaluations
        public int NombreEvaluations
        {
            get => _nombreEvaluations;
            set
            {
                if (_nombreEvaluations != value)
                {
                    _nombreEvaluations = value;
                    OnPropertyChanged();
                }
            }
        }

        public Livre()
        {
        }

        public Livre(string titre, string auteur, string isbn)
        {
            Titre = titre;
            Auteur = auteur;
            ISBN = isbn;
        }

        // Événement requis par l'interface INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        // Méthode utilitaire pour déclencher l'événement
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Ajoute une nouvelle évaluation.
        /// Formule : ((Ancienne moyenne * N) + Nouvelle note) / (N + 1)
        /// </summary>
        public void AjouterEvaluation(double nouvelleNote)
        {
            if (nouvelleNote < 0 || nouvelleNote > 5)
                throw new ArgumentOutOfRangeException(nameof(nouvelleNote));

            double somme = (MoyenneEvaluation * NombreEvaluations) + nouvelleNote;

            // On met à jour via les propriétés pour déclencher les notifications
            NombreEvaluations++;
            MoyenneEvaluation = somme / NombreEvaluations;
        }

        /// <summary>
        /// Modifie une évaluation existante.
        /// Formule : ((Ancienne moyenne * N) - Ancienne note + Nouvelle note) / N
        /// </summary>
        public void ModifierEvaluation(double ancienneNote, double nouvelleNote)
        {
            if (NombreEvaluations <= 0)
            {
                AjouterEvaluation(nouvelleNote);
                return;
            }

            if (nouvelleNote < 0 || nouvelleNote > 5)
                throw new ArgumentOutOfRangeException(nameof(nouvelleNote));

            // Note : NombreEvaluations ne change pas ici, mais la moyenne oui
            double somme = (MoyenneEvaluation * NombreEvaluations) - ancienneNote + nouvelleNote;

            MoyenneEvaluation = somme / NombreEvaluations;
        }

        // Retourne vrai si la note est >= 4.0
        // Cette méthode est maintenant liée dynamiquement via OnPropertyChanged dans le setter de MoyenneEvaluation
        public bool EstFavori() => MoyenneEvaluation >= 4.0;
    }
}
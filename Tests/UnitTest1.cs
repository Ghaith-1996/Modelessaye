using NUnit.Framework;
using Bibliotheque.Model;

namespace Tests
{
    public class LivreTests
    {
        private Livre _livre;

        [SetUp]
        public void Setup()
        {
            // Initialisation d'un livre vide avant chaque test pour partir d'un état propre
            _livre = new Livre("Titre Test", "Auteur Test", "123-456");
        }

        [Test]
        public void AjouterEvaluation_PremiereNote_CalculeMoyenneCorrectement()
        {
            // Arranger : État initial (0 note)
            _livre.MoyenneEvaluation = 0;
            _livre.NombreEvaluations = 0;

            // Agir : Ajout d'une note de 5.0
            // Formule : ((0 * 0) + 5) / 1 = 5
            _livre.AjouterEvaluation(5.0);

            // Auditer (Assert)
            Assert.That(_livre.NombreEvaluations, Is.EqualTo(1));
            Assert.That(_livre.MoyenneEvaluation, Is.EqualTo(5.0));
        }

        [Test]
        public void AjouterEvaluation_NouvelleNote_AppliqueFormulePonderee()
        {
            // Arranger : Supposons une moyenne de 4.0 basée sur 1 note (par exemple un 4/5)
            _livre.MoyenneEvaluation = 4.0;
            _livre.NombreEvaluations = 1;

            // Agir : Ajout d'une note de 2.0
            // Formule attendue : ((4.0 * 1) + 2.0) / (1 + 1) = 6 / 2 = 3.0
            _livre.AjouterEvaluation(2.0);

            // Auditer
            Assert.That(_livre.NombreEvaluations, Is.EqualTo(2));
            Assert.That(_livre.MoyenneEvaluation, Is.EqualTo(3.0));
        }

        [Test]
        public void ModifierEvaluation_MiseAJourMoyenne_SansChangerNombreTotal()
        {
            // Arranger : Supposons une moyenne de 3.0 basée sur 2 notes (ex: notes 4 et 2)
            _livre.MoyenneEvaluation = 3.0;
            _livre.NombreEvaluations = 2;

            double ancienneNote = 2.0; // La note qu'on veut corriger
            double nouvelleNote = 5.0; // La nouvelle note (on change le 2 en 5)

            // Agir
            // Formule TP : ((Moyenne * N) - Ancienne + Nouvelle) / N
            // Calcul : ((3.0 * 2) - 2.0 + 5.0) / 2
            //        = (6 - 2 + 5) / 2 
            //        = 9 / 2 
            //        = 4.5
            _livre.ModifierEvaluation(ancienneNote, nouvelleNote);

            // Auditer
            Assert.That(_livre.NombreEvaluations, Is.EqualTo(2), "Le nombre d'évaluations ne doit pas changer lors d'une modification.");
            Assert.That(_livre.MoyenneEvaluation, Is.EqualTo(4.5), "La moyenne doit être recalculée selon la formule spécifique.");
        }

        [Test]
        public void EstFavori_RetourneVrai_SiMoyenneEgaleOuSuperieureA4()
        {
            // Cas limite : exactement 4.0
            _livre.MoyenneEvaluation = 4.0;
            Assert.IsTrue(_livre.EstFavori(), "Un livre avec 4.0 doit être favori.");

            // Cas supérieur : 4.9
            _livre.MoyenneEvaluation = 4.9;
            Assert.IsTrue(_livre.EstFavori(), "Un livre avec 4.9 doit être favori.");
        }

        [Test]
        public void EstFavori_RetourneFaux_SiMoyenneInferieureA4()
        {
            // Cas limite inférieur : 3.9
            _livre.MoyenneEvaluation = 3.9;
            Assert.IsFalse(_livre.EstFavori(), "Un livre avec 3.9 ne doit pas être favori.");
        }
    }
}
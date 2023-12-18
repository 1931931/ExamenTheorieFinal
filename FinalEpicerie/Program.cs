// See https://aka.ms/new-console-template for more information
using StockLibrary;
using System.Threading.Channels;

class Program
{
    static void Main(string[] args)
    {
        StockService stock = new StockService();
        List<Produit> produitList = new List<Produit>();
        produitList = stock.CorrigerPrixAvantTaxes("1");
        stock.MettreaJourMesProduits(produitList);
        bool close = false;


        while(!close)
        {
            int reponse = SaisirEntier("Bienvenue à l'épicerie!\n\t1- Mise à jour d'un prix de produit\n\t" +
                "2- Affichage de la lsite des produits\n\t3-Statistiques de l'inventaire\n\t 4-Déconnexion : ", 1, 4);

            //Mise à jour d'un prix
            if (reponse == 1)
            {
                string codeProduit = SaisirString("Saisir le code du produit à modifier : ");
                double nouvPrix = SaisirDoublePositif("Saisir le nouveau prix : ");
                foreach (Produit produit in produitList)
                {
                    if (codeProduit == produit.CodeProduit)
                    {
                        produit.PrixAvantTaxes = nouvPrix;
                        produit.PrixApresTaxes = nouvPrix + (nouvPrix * produit.TPS) + (nouvPrix * produit.TVQ);
                    }
                }
                stock.MettreaJourMesProduits(produitList);
            }
            //Affichage de la liste
            else if (reponse == 2)
            {
                Console.WriteLine("Liste des produits : ");
                stock.ListerLesProduits();
            }
            //Statistiques de l'inventaire
            else if (reponse == 3)
            {
                //variables
                int nbTotal = 0;
                int nbSansTaxe = 0;
                double totalAvantTaxe = 0;
                double totalApresTaxe = 0;

                //traitement de la liste
                foreach (Produit produit in produitList)
                {
                    nbTotal++;
                    totalAvantTaxe += produit.PrixAvantTaxes;
                    totalApresTaxe += produit.PrixApresTaxes;

                    if (produit.TPS == 0 && produit.TVQ == 0)
                        nbSansTaxe++;
                }
                //affichage
                Console.WriteLine("Inventaire : ");
                Console.WriteLine($"Nombre total de produits : {nbTotal} ");
                Console.WriteLine($"Nombre de produits exonérés de taxes : {nbSansTaxe}");
                Console.WriteLine($"Prix total de l'inventaire avant taxes : {totalAvantTaxe}");
                Console.WriteLine($"Prix total de l'inventaire après taxes : {totalApresTaxe}");
            }
            //Déconnexion
            else if (reponse == 4)
            {
                close = true;
            }
        }
    }


    /// <summary>
    /// Méthode dé vérification de int avec minimum et maximum
    /// </summary>
    /// <param name="message"></param>
    /// <param name="min">inclus</param>
    /// <param name="max">inclus</param>
    /// <returns>Un int du minimu au maximum</returns>
    private static int SaisirEntier(string message, int min, int max)
    {
        Console.Write(message);
        int entier;
        while (int.TryParse(Console.ReadLine(), out entier) == false || entier < min || entier > max)
            Console.Write("Ceci n'est pas un entier valide. Réessayez : ");
        return entier;
    }

    /// <summary>
    /// Vérifie que la string n'est pas null
    /// </summary>
    /// <param name="message">Message pour l'utilisateur</param>
    /// <returns>une string non-null</returns>
    private static string SaisirString(string message)
    {
        Console.Write(message);
        string reponse = Console.ReadLine();
        while (reponse == "")
        {
            Console.WriteLine("Veuillez entrer une valeur.");
            reponse = Console.ReadLine();
        }
        return reponse;
    }
    // Double positif avec message
    private static double SaisirDoublePositif(string message)
    {
        double valide;
        Console.Write(message);

        while (double.TryParse(Console.ReadLine(), out valide) == false || valide < 0)
            Console.Write("Ceci n'est pas un montant positif valide. Réessayez : ");
        return valide;
    }
    // Double avec min et max inclus

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{

    class Program
    {
        const String ConnectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Nicolas Chourot\Desktop\Cinema\App_Data\Cinema.mdf';Integrated Security=True";
        static public void Ajouter_Film(String titre, DateTime parution, Cinema.Genre genre)
        {
            Films films = new Films(ConnectionString);
            films.Titre = titre;
            films.Parution = parution;
            films.Genre = genre;
            films.Insert();
        }

        static public void Ajouter_Acteur(String film_Titre, String nom, DateTime naissance, String nationalite)
        {
            Acteurs acteurs = new Acteurs(ConnectionString);
            acteurs.Nom = nom;
            acteurs.Naissance = naissance;
            acteurs.Nationalite = nationalite;
            acteurs.Insert();

            Films films = new Films(ConnectionString);
            if (films.SelectByFieldName("Titre", film_Titre))
            {
                Parutions parutions = new Parutions(ConnectionString);

                acteurs.SelectLast();
                parutions.Acteur_Id = acteurs.Id;

                parutions.Film_Id = films.Id;

                parutions.Insert();
            }
        }

        static public void Peupler_Cinema()
        {
            Ajouter_Film("La ligne verte", new DateTime(1999, 12, 10), Cinema.Genre.drame);
            Ajouter_Acteur("La ligne verte", "Tom Hanks", new DateTime(1956, 7, 9), "USA");
            Ajouter_Acteur("La ligne verte", "David Morse", new DateTime(1953, 10, 11), "USA");

            Ajouter_Film("La matrice", new DateTime(1999, 3, 31), Cinema.Genre.fiction);
            Ajouter_Acteur("La matrice", "Keanu Reeves", new DateTime(1964, 9, 2), "Lebanon");
            Ajouter_Acteur("La matrice", "Carrie-Anne Moss", new DateTime(1967, 8, 21), "Canada");

            Ajouter_Film("Retour vers le futur", new DateTime(1985, 7, 3), Cinema.Genre.comédie);
            Ajouter_Acteur("Retour vers le futur", "Michael J. Fox", new DateTime(1961, 6, 9), "Canada");
            Ajouter_Acteur("Retour vers le futur", "Christopher Lloyd", new DateTime(1938, 10, 22), "USA");
            Ajouter_Acteur("Retour vers le futur", "Lea Thompson", new DateTime(1961, 5, 31), "USA");
        }

        static public void Lister_Films()
        {
            Films films = new Films(ConnectionString);
            if (films.SelectAll("Parution"))
            {
                Console.WriteLine("Les des films");
                do
                {
                    Console.WriteLine("********************************");
                    Console.WriteLine(films.Titre);
                    Console.WriteLine(films.Parution.ToLongDateString());
                    Console.WriteLine(films.Genre.ToString());
                } while (films.Next());
                Console.WriteLine("********************************");
            }
        }

        static public void Lister_Acteurs()
        {
            Acteurs acteurs = new Acteurs(ConnectionString);
            if (acteurs.SelectAll("Naissance"))
            {
                Console.WriteLine("Les des acteurs"); 
                do
                {

                    Console.WriteLine("********************************");
                    Console.WriteLine(acteurs.Nom);
                    Console.WriteLine(acteurs.Naissance.ToLongDateString());
                    Console.WriteLine(acteurs.Nationalite.ToString());
                } while (acteurs.Next());
                Console.WriteLine("********************************");
            }
        }

        static public void Lister_Acteurs_Par_Film(String film_titre)
        {
            Acteurs_Par_Film apf = new Acteurs_Par_Film(film_titre, ConnectionString);

            if (apf.SelectAll("Naissance"))
            {

                Console.WriteLine("Les des acteurs du film [" + film_titre + "]"); 
                do
                {
                    Console.WriteLine("********************************");
                    Console.WriteLine(apf.Nom);
                    Console.WriteLine(apf.Naissance.ToLongDateString());
                    Console.WriteLine(apf.Nationalite.ToString());
                } while (apf.Next());
                Console.WriteLine("********************************");
            }

        }

        static void Main(string[] args)
        {
            //Peupler_Cinema();
            //Lister_Films();
            //Lister_Acteurs();
            //Lister_Acteurs_Par_Film("Retour vers le futur");
        }
    }
}

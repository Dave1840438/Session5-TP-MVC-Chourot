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
            films.film.Titre = titre;
            films.film.Parution = parution;
            films.film.Genre = genre;
            films.Insert();
        }
        
        
        static public void Ajouter_Acteur(String film_Titre, String nom, DateTime naissance, String nationalite)
        {
            Acteurs acteurs = new Acteurs(ConnectionString);
            acteurs.acteur.Nom = nom;
            acteurs.acteur.Naissance = naissance;
            acteurs.acteur.Nationalite = nationalite;
            acteurs.Insert();

            Films films = new Films(ConnectionString);
            if (films.SelectByFieldName("Titre", film_Titre))
            {
                Parutions parutions = new Parutions(ConnectionString);

                acteurs.SelectLast();
                parutions.parution.Acteur_Id = acteurs.acteur.Id;
                parutions.parution.Film_Id = films.film.Id;
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
            films.SelectAll("Parution");

            Console.WriteLine("Listes des films");
            /*
             while (films.Next())
            {
                Console.WriteLine("********************************");
                Console.WriteLine(films.film.Titre);
                Console.WriteLine(films.film.Parution.ToLongDateString());
                Console.WriteLine(films.film.Genre.ToString());
            } 
            */
            
            List<Object> films_list = films.ToList();
            foreach(Film film in films_list)
            {
                Console.WriteLine("********************************");
                Console.WriteLine(film.Titre);
                Console.WriteLine(film.Parution.ToLongDateString());
                Console.WriteLine(film.Genre.ToString());
            }
            Console.WriteLine("********************************");
           
        }

        
        static public void Lister_Acteurs()
        {
            Acteurs acteurs = new Acteurs(ConnectionString);

            acteurs.SelectAll("Naissance");
            Console.WriteLine("Listes des acteurs");
            while (acteurs.Next())
            {
                Console.WriteLine("********************************");
                Console.WriteLine(acteurs.acteur.Nom);
                Console.WriteLine(acteurs.acteur.Naissance.ToLongDateString());
                Console.WriteLine(acteurs.acteur.Nationalite.ToString());
            }
            Console.WriteLine("********************************");
        }
        
        
        static public void Lister_Acteurs_Par_Film(String film_titre)
        {
            Acteurs_Par_Film apf = new Acteurs_Par_Film(film_titre, ConnectionString);

            apf.SelectAll("Naissance");
            List<object> list = apf.ToList();
            Console.WriteLine("Listes des acteurs du film [" + film_titre + "]");
            foreach(Acteur acteur in list)
            {
                Console.WriteLine("********************************");
                Console.WriteLine(acteur.Nom);
                Console.WriteLine(acteur.Naissance.ToLongDateString());
                Console.WriteLine(acteur.Nationalite.ToString());
            }
            Console.WriteLine("********************************");

        }
        
        static void Main(string[] args)
        {
            //Peupler_Cinema();
            Lister_Films();
            Lister_Acteurs();
            Lister_Acteurs_Par_Film("Retour vers le futur");
        }
    }
}

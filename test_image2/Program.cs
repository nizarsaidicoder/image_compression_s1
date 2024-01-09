﻿// Auteur : Aurélie Leborgne
// Calcule la SEDT et les boules maximales d'une forme
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Schema;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Linq;

namespace test_image2
{
    // Structure représentant une boule avec un centre (X, Y) et un rayon.
    class ChartControl : Control
    {
        private readonly Chart _chart;

        public ChartControl(Chart chart)
        {
            _chart = chart;
            _chart.Dock = DockStyle.Fill;
            Controls.Add(_chart);
        }
    }
    struct Boule
    {
        public int X;
        public int Y;
        public int Rayon;

        public Boule(int x, int y, int rayon)
        {
            X = x;
            Y =  y;
            Rayon = rayon;
        }

        // Affiche les infos de la boules
        public void AfficherInfo()
        {
            Console.WriteLine($"Centre de la boule : ({X}, {Y})");
            Console.WriteLine($"Rayon de la boule : {Rayon}");
        }
        /// <summary>
        /// Vérifie si la boule est incluse dans une autre boule.
        /// </summary>
        /// <param name="Xboule">La boule à comparer.</param>
        /// <returns>Retourne vrai si la boule est incluse, sinon faux.</returns>
        public bool EstIncluse(Boule Xboule)
        {
            // Calcul de la distance entre les centres des deux boules
            int distance = (int)(Math.Pow((X - Xboule.X), 2) + Math.Pow((Y - Xboule.Y), 2));

            // Vérification de l'inclusion en comparant la somme des rayons avec la distance
            return distance + Rayon <= Xboule.Rayon;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /*********************************** INITIALISTION ***********************************/
            // Spécifie le chemin d'accès à votre image BMP

            string imagePath = $"../../images/imagesReelles/lotus.txt";
            //Transforme l'image en tableau 2D
            //int[,] tabImage = TabFromFile(imagePath);
            /*
            int[,] tabImage = {
                {0, 255, 0, 255, 55, 255, 0,55},
                {0, 255, 0, 55, 255, 255, 0, 255},
                {0, 55, 0, 255, 4, 255, 0, 2},
                {4, 255, 0, 255, 4, 255, 0, 255},
                {0, 55, 0, 255, 0, 255, 0, 255},
                {0, 255, 0, 255, 255, 255, 0, 255},
                {0, 255, 0, 255, 255, 255, 0, 255},
                    {0, 255, 0, 255, 255, 255, 0, 255},
                {0, 255, 0, 255, 255, 5, 0, 255},
                {0, 255, 0, 255, 255, 255, 0, 255},
                {0, 25, 0, 255, 255, 255, 0, 255},
                {0, 25, 0, 25, 255, 255, 0, 255},
                {0, 255, 0, 255, 255, 255, 0, 255},
                {0, 25, 0, 255, 255, 255, 0, 255},
                {0, 255, 0, 25, 255, 255, 0, 255},
                {0, 255, 0, 55, 255, 255, 0, 255},
                {0, 255, 0, 255, 255, 255, 0, 255},
                {0, 255, 0, 255, 255, 255, 0, 205},
                {0, 255, 0, 255, 55, 255, 0, 255},
            };
            */

            //MesurerTempsCalculCarteDistanceOptiBruit();
            //WatchBouleMaxNoisePerformance(ExtraireBoulesMax, "Optimise");
            //WatchSEDTFormePixelPerformance(CarteOptimise, "Optimise");
            /************************************************************************************/
            // Cree un tableau à partir de tabImage normalisee en deux valeurs (0 et "infini")
            //int[,] tabInit = InitResultat(tabImage);
            /************************* METHODE BRUTE FORCE *************************/
            // Console.WriteLine("METHODE BRUTE FORCE");
            // int resultatBrute = CarteBruteForce(tabInit);
            // Affiche_image(resultatBrute);
            // SaveImage(resultatBrute, "../../images/RESULTATS_BRUTE/resultatBrute.bmp");
            /**********************************************************************/

            /************************* METHODE OPTIMISEE *************************/
            //Console.WriteLine("METHODE OPTIMISEE");
            //int[,] resultatOptimise = CarteOptimise(tabInit);

            /************************* Boules Maximales *************************/

            //List<Boule> BoulesMax = ExtraireBoulesMax(resultatOptimise);
            /*
            List<Boule> BoulesMax = TrouverBoulesMax(resultatOptimise);
            //CompareListesBoules(BoulesMax, BoulesMaxBrute);
             
            int width = tabImage.GetLength(0);
            int height = tabImage.GetLength(1);
            BoulesMaxToFile(BoulesMax, width, height, imagePath);
            Affiche_Squelette(tabImage,BoulesMax);
            SaveSquelette(tabImage,BoulesMax,$"../../images/image.bmp");
            */
            Reconstruction(imagePath);
            ComparerImagesPourcentage("../../images/image.bmp", "../../images/imagesReelles/lotus.bmp");
            Console.ReadKey(); 
        }
        static bool CompareListesBoules(List<Boule> liste1, List<Boule> liste2)
        {
            // Si elles n'ont pas la meme longueur c'est qu'elles sont différentes
            if (liste1.Count != liste2.Count)
            {
                Console.WriteLine("nb boule dans l'optimisé: " + liste1.Count);
                Console.WriteLine("nb boule dans la brute force: " + liste2.Count);
                return false;
            }

            // On vérifie chaque boule dans les deux listes
            for (int i = 0; i < liste1.Count; i++)
            {
                // On compare les Rayon de deux boules
                if (liste1[i].Rayon != liste2[i].Rayon)
                {
                    return false;
                }
            }
            return true;
        }
        public static List<Boule> TrouverBoulesMax(int[,] distanceEucCarre)
        {
            List<Boule> boules = new List<Boule>();

            for (int x1 = 0; x1 < distanceEucCarre.GetLength(0); x1++)
            {
                for (int y1 = 0; y1 < distanceEucCarre.GetLength(1); y1++)
                {
                    bool estMax = true;
                    int rayon = distanceEucCarre[x1, y1];
                    if (rayon > 0)
                    {
                        for (int x2 = 0; x2 < distanceEucCarre.GetLength(0) && estMax; x2++)
                        {
                            for (int y2 = 0; y2 < distanceEucCarre.GetLength(1) && estMax; y2++)
                            {
                                int rayon2 = distanceEucCarre[x2, y2];
                                if ((x1 != x2 || y1 != y2) && rayon2 > 0)
                                {
                                    int distance = (int)(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));

                                    // Si la boule x1 y1 est incluse dans x2 y2, elle n'est pas maximale
                                    if (rayon + distance <= rayon2)
                                    {
                                        estMax = false;
                                    }
                                }

                            }
                        }
                        if (estMax)
                        {
                            boules.Add(new Boule(x1, y1, distanceEucCarre[x1, y1]));
                        }
                    }
                }
            }
            TrierBoulesParRayonDecroissant(boules);
            return boules;
        }
        /********************************************************************************************
        *********************************************************************************************
        **************************     CARTE DE DISTANCE EUCLIDIENNE      ***************************
        *********************************************************************************************
        *********************************************************************************************
        ********************************************************************************************/


        /**************************                             **************************************
        ***************************     METHODE BRUTE FORCE     **************************************
        ***************************                             **************************************/
        /// <summary>
        /// Génère une carte de distances avec la methode brute force à partir d'un tableau 2D de pixels.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D d'entiers normalisé à partir duquel générer la carte.</param>
        /// <returns>Le tableau 2D représentant la carte de distances </returns>
        public static int[,] CarteBruteForce(int[,] Xtab)
        {
            int hauteur = Xtab.GetLength(0);
            int largeur = Xtab.GetLength(1);
            int[,] resultat = new int[hauteur, largeur];
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int col = 0; col < largeur; col++)
                {
                    int pixel = Xtab[ligne, col];
                    int min = pixel;
                    int xP = ligne;
                    int yP = col;
                    // Vérifie si le pixel n'est pas dans le fond
                    if (pixel != 0)
                    {
                        for (int ligne2 = 0; ligne2 < hauteur; ligne2++)
                        {
                            for (int col2 = 0; col2 < largeur; col2++)
                            {
                                int pixel2 = Xtab[ligne2, col2];
                                // Vérifie si le pixel2 est dans le fond
                                if (pixel2 == 0)
                                {
                                    int distance = (int)(Math.Pow(ligne2 - xP, 2) + Math.Pow(col2 - yP, 2));
                                    if (distance < min) min = distance;
                                }
                            }
                            // Attribue la distance minimale au pixel d'origine
                            resultat[ligne, col] = min;
                        }
                    }
                }
            }
            return resultat;
        }


        /**************************                             **************************************
        ***************************   METHODE SEMI BRUTE FORCE  **************************************
        ***************************                             **************************************/

        /// <summary>
        /// Génère une carte de distances avec la methode semi-brute force à partir d'un tableau 2D de pixels.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D d'entiers normalisé à partir duquel générer la carte.</param>
        /// <returns>Le tableau 2D représentant la carte de distances </returns>
        public static int[,] CarteSemiBruteForce(int[,] Xtab)
        {
            List<int[]> indicePixelFond = TrouveIndice(Xtab);
            int hauteur = Xtab.GetLength(0);
            int largeur = Xtab.GetLength(1);
            int[,] resultat = new int[hauteur, largeur];
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int col = 0; col < largeur; col++)
                {
                    int pixel = Xtab[ligne, col];
                    int min = pixel;
                    int xP = ligne;
                    int yP = col;
                    if (pixel != 0)
                    {
                        foreach (int[] indice in indicePixelFond)
                        {
                            int xQ = indice[0];
                            int yQ = indice[1];
                            int distance = (int)(Math.Pow(xQ - xP, 2) + Math.Pow(yQ - yP, 2));
                            if (distance < min) min = distance;
                        }
                        resultat[ligne, col] = min;
                    }
                }
            }
            return resultat;
        }

        /**************************                             **************************************
        ***************************      METHODE OPTIMISEE      **************************************
        ***************************                             **************************************/

        /// <summary>
        /// Génère une carte de distances avec une méthode optimisé (PROPAGATION VERTICALE/HORIZONTALE) à partir d'un tableau 2D de pixels.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D d'entiers sur lequel effectuer la propagation.</param>
        /// <returns>Le tableau 2D représentant la carte de distances</returns>

        public static int[,] CarteOptimise(int[,] Xtab)
        {
            int[,] tabResultats = PropagationVerticale(Xtab);
            tabResultats = PropagationHorizontale(tabResultats);
            return tabResultats;
        }

        /********************************************************************************************
       *********************************************************************************************
       **********************************     RECONSTRUCTION     ***********************************
       *********************************************************************************************
       *********************************************************************************************
       ********************************************************************************************/

        public static void Reconstruction(string TextPath)
        {

            // Read all lines from the file
            string[] lines = File.ReadAllLines(TextPath);

            // Parse the first line to get width and height
            string[] dimensions = lines[0].Split(',');
            int width = int.Parse(dimensions[0].Trim());
            int height = int.Parse(dimensions[1].Trim());
            Console.WriteLine($"{width},{height}");
            // Create a new Bitmap image with the specified width and height
            Bitmap bitmap = new Bitmap(width, height);
            int[,] tab = new int[width, height];
            // Create a list to store Boule structures
            List<Boule> BoulesMax = new List<Boule>();

            // Process the other arrays and create Boule structures
            for (int i = 1; i < lines.Length; i++)
            {
                // Remove brackets from each line
                string lineWithoutBrackets = lines[i].Replace("[", "").Replace("]", "");

                string[] values = lineWithoutBrackets.Split(',');

                // Assuming the array has three elements (X, Y, and Rayon)
                int x = int.Parse(values[0].Trim());
                int y = int.Parse(values[1].Trim());
                int rayon = int.Parse(values[2].Trim());

                // Create a new Boule structure and add it to the list
                Boule boule = new Boule(x, y, rayon);
                BoulesMax.Add(boule);
            }
            Affiche_Squelette(tab, BoulesMax);
            SaveSquelette(tab,BoulesMax, "../../images/image.bmp");
            /*
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(bitmap, 0, 0);
                
                for (int lig = 0; lig < height; lig++)
                {
                    for (int col = 0; col < width; col++)
                    {
                        int valeurPixel = Xtab[lig, col];
                        Color finalColor = Color.FromArgb(255, 255, 255, 255); ;
                        // if (valeurPixel == 255) finalColor = Color.FromArgb(0, 0, 0, 0);
                        //else finalColor = Color.FromArgb(255, valeurPixel, valeurPixel, valeurPixel);
                        Ximg.SetPixel(col, lig, finalColor);
                    }
                }
                
                foreach (Boule boule in BoulesMax)
                {

                    int rayon = (int)Math.Round(Math.Sqrt(boule.Rayon));
                    Color circleGradientColor = CalculateGradientColor(boule.X, width);
                    // Dessin des cercles représentant les boules maximales
                    //DrawCircle(g, boule.Y, boule.X, rayon, circleGradientColor);

                    SolidBrush blackBrush = new SolidBrush(Color.Black);

                    g.FillEllipse(blackBrush, boule.Y - rayon, boule.X - rayon, 2 * rayon, 2 * rayon);

                    // Make sure to dispose of the brush to free up resources
                    blackBrush.Dispose();
                }
                Form f = new Form();
                f.BackgroundImage = bitmap;
                f.Width = bitmap.Width;
                f.Height = bitmap.Height;
                f.Show();
            }
        */
        }
        /// <summary>
        /// Extraire les boules maximales à partir d'un tableau 2D représentant une carte distance.
        /// </summary>
        /// <param name="carteDistance">La carte de distance (Tableau 2D).</param>
        /// <returns>Une liste de boules maximales extraites de la carte.</returns>
        public static List<Boule> ExtraireBoulesMax(int[,] carteDistance)
        {
            List<Boule> boules = CreerListeBoules(carteDistance);
            TrierBoulesParRayonDecroissant(boules);

            return TrouverBoulesMaximales(boules);
        }

        /// <summary>
        /// Écrit les informations sur une liste de Boules dans un fichier texte.
        /// </summary>
        /// <param name="Xboules">La liste de Boules à écrire dans le fichier.</param>
        /// <param name="Xwidth">La largeur de l'image ou de la zone.</param>
        /// <param name="Xheight">La hauteur de l'image ou de la zone.</param>
        /// <param name="Xpath">Le chemin du fichier de sortie.</param>
        public static void BoulesMaxToFile(List<Boule> Xboules, int Xwidth, int Xheight, string Xpath)
        {
            string path = Xpath.Substring(0, Xpath.Length - 3);
            path += "txt";
            string resultat = "";
            resultat += $"{Xwidth}, {Xheight}\n";
            foreach (Boule boule in Xboules)
            {
                string bouleInfo = "[";
                bouleInfo += $"{boule.X}, ";
                bouleInfo += $"{boule.Y}, ";
                bouleInfo += $"{boule.Rayon}]\n";
                resultat += bouleInfo;
            }
            using (FileStream output = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(output))
                {
                    writer.Write(resultat);
                }
            }
            //FileStream output = new FileStream("../../images/imagesReelles/malenia.bmp")
        }

        /********************************************************************************************
        *********************************************************************************************
        ********************************     HELPER FUNCTIONS      **********************************
        *********************************************************************************************
        *********************************************************************************************
        ********************************************************************************************/

        static double ComparerImagesPourcentage(string cheminImageOriginale, string cheminImageReconstruite)
        {
            Bitmap imageOriginale = null;
            Bitmap imageReconstruite = null;

            // On fait une vérification pour voir si les chemins fournis sont bons
            try
            {
                imageOriginale = new Bitmap(cheminImageOriginale);
                imageReconstruite = new Bitmap(cheminImageReconstruite);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des images : {ex.Message}");
                return 0.0; // Ou une valeur appropriée en cas d'échec du chargement des images
            }

            // On vérifie les dimensions des images et redimensionner si nécessaire
            if (imageOriginale.Width != imageReconstruite.Width || imageOriginale.Height != imageReconstruite.Height)
            {
                imageReconstruite = new Bitmap(imageReconstruite, imageOriginale.Size);
            }

            int pixelsIdentiques = 0;
            int totalPixels = imageOriginale.Width * imageOriginale.Height;

            //On compare chaque pixel des deux images
            for (int x = 0; x < imageOriginale.Width; x++)
            {
                for (int y = 0; y < imageOriginale.Height; y++)
                {
                    if (imageOriginale.GetPixel(x, y) == imageReconstruite.GetPixel(x, y))
                    {
                        pixelsIdentiques++;
                    }
                }
            }

            Console.WriteLine("Boucle de comparaison terminée");

            // On calcule le pourcentage de reconstruction
            double pourcentageReconstruction = (double)pixelsIdentiques / totalPixels * 100;
            Console.WriteLine($"Pourcentage de reconstruction : {pourcentageReconstruction}%");
            return pourcentageReconstruction;
        }
        public static int GetFormPixels(int[,] XTab)
        {
            int hauteur = XTab.GetLength(0);
            int largeur = XTab.GetLength(1);

            int nbPixelForme = 0; 
            for (int lig = 0; lig < hauteur; lig++)
            {
                for (int col = 0; col < largeur; col++)
                {
                    if (XTab[lig, col] != 0) nbPixelForme++;
                }
            }
            return nbPixelForme;
        }
        public static int GetFondPixels(int[,] XTab)
        {
            int hauteur = XTab.GetLength(0);
            int largeur = XTab.GetLength(1);

            int nbPixelFond = 0;
            for (int lig = 0; lig < hauteur; lig++)
            {
                for (int col = 0; col < largeur; col++)
                {
                    if (XTab[lig, col] == 0) nbPixelFond++;
                }
            }
            return nbPixelFond;
        }
        static int TrieCheminImagePixelForme(string imagePath)
        {
            int[,] tabImage = TabFromFile(imagePath);
            int[,] tabInit = InitResultat(tabImage);
            int nbPixelForme = GetFormPixels(tabInit);
            return nbPixelForme;
        }
        static int TrieCheminImagePixelFond(string imagePath)
        {
            int[,] tabImage = TabFromFile(imagePath);
            int[,] tabInit = InitResultat(tabImage);
            int nbPixelFond = GetFondPixels(tabInit);

            return nbPixelFond;
        }

        static int TrieCheminImagePixelTotal(string imagePath)
        {
            int[,] tabImage = TabFromFile(imagePath); 
            int nombreTotalPixels = tabImage.GetLength(0) * tabImage.GetLength(1);
            return nombreTotalPixels;
        }

        /// <summary>
        /// Affiche un tableau 2D d'entiers dans la console.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D d'entiers à afficher.</param>
        public static void AfficheTableau2D(int[,] Xtab)
        {
            int nbLig = Xtab.GetLength(0);
            int nbCol = Xtab.GetLength(1);
            for (int row = 0; row < nbLig; row++)
            {
                Console.Write("[");
                for (int col = 0; col < nbCol; col++)
                {
                    if (Xtab[row, col] >= 10)
                    {
                        Console.Write($"{Xtab[row, col]} ");
                    }
                    else
                    {
                        Console.Write($" {Xtab[row, col]} ");
                    }
                    if (col < nbCol - 1)
                    {
                        Console.Write("|");
                    }
                }
                Console.Write("]");
                if (row < nbLig - 1)
                {
                    Console.WriteLine();
                }
            }
        }

        ///<summary>
        ///Affiche tous les boules maximales.
        ///</summary>
        ///<param name = "boules" > Liste des boules</param>
        public static void AfficherBoules(List<Boule> boules)
        {
            foreach (var boule in boules)
            {
                Console.WriteLine("Boule Maximale:");
                boule.AfficherInfo();
                Console.WriteLine("--------------------");
            }
        }

        /// <summary>
        /// Recherche la valeur maximale dans un tableau 2D.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D dans lequel rechercher la valeur maximale.</param>
        /// <returns>La valeur maximale trouvée dans le tableau.</returns>
        public static int TrouveValMax(int[,] Xtab)
        {
            int max = Xtab[0, 0];

            foreach (int value in Xtab)
            {
                if (value > max) max = value;
            }

            return max;
        }

        /// <summary>
        /// Compare deux tableaux 2D d'entiers et compte le nombre de différences.
        /// </summary>
        /// <param name="Xtab1">Le premier tableau 2D d'entiers.</param>
        /// <param name="Xtab2">Le deuxième tableau 2D d'entiers à comparer.</param>
        /// <returns>Le nombre de différences entre les deux tableaux.</returns>
        public static int Comparaison(int[,] Xtab1, int[,] Xtab2)
        {
            int compteur = 0;

            for (int i = 0; i < Xtab1.GetLength(0); i++)
            {
                for (int j = 0; j < Xtab1.GetLength(1); j++)
                {
                    if (Xtab1[i, j] != Xtab2[i, j]) compteur++;
                }
            }

            return compteur;
        }

        /// <summary>
        /// Normalise les valeurs d'un tableau 2D d'entiers 0 ou 255.
        /// </summary>
        /// <param name="XTab">Le tableau 2D d'entiers à normaliser.</param>
        public static void Normalise(int[,] XTab)
        {
            for (int ligne = 0; ligne < XTab.GetLength(0); ligne++)
            {
                for (int col = 0; col < XTab.GetLength(1); col++)
                {
                    int pixel = XTab[ligne, col];
                    if (pixel < 128 && pixel > 0) pixel = 0;
                    else if (pixel < 255 && pixel > 127) pixel = 255;
                    XTab[ligne, col] = pixel;
                }
            }
        }

        /// <summary>
        /// Initialise un tableau résultat pour calculer la carte de distance, en attribuant 0 au pixel du fond et une valeur maximale aux pixels de la forme
        /// </summary>
        /// <param name="XTab">Le tableau 2D d'entiers à partir duquel initialiser le résultat.</param>
        /// <returns>Le tableau résultat initialisé.</returns>
        public static int[,] InitResultat(int[,] Xtab)
        {
            int[,] resultat = CopieTab2D(Xtab);
            Normalise(resultat);

            int max = (int)(Math.Pow((resultat.GetLength(0) - 1), 2) + Math.Pow((resultat.GetLength(1) - 1), 2));
            for (int ligne = 0; ligne < resultat.GetLength(0); ligne++)
            {
                for (int col = 0; col < resultat.GetLength(1); col++)
                {
                    int pixel = resultat[ligne, col];
                    if (pixel == 255) pixel = 0;
                    else pixel = max;
                    resultat[ligne, col] = pixel;
                }
            }
            return resultat;
        }

        /// <summary>
        /// Trouve les indices des pixels de fond dans un tableau 2D d'entiers.
        /// </summary>
        /// <param name="XTab">Le tableau 2D d'entiers à partir duquel trouver les indices.</param>
        /// <returns>Une liste de tableau de cordonnées (x,y) des pixels de fond.</returns>
        public static List<int[]> TrouveIndice(int[,] Xtab)
        {
            List<int[]> resultat = new List<int[]>();
            for (int ligne = 0; ligne < Xtab.GetLength(0); ligne++)
            {
                for (int col = 0; col < Xtab.GetLength(1); col++)
                {
                    int pixel = Xtab[ligne, col];
                    if (pixel == 0)
                    {
                        int[] newPixel = new int[] { ligne, col };
                        resultat.Add(newPixel);
                    }
                }
            }
            return resultat;
        }

        /// <summary>
        /// Copie un tableau 1D.
        /// </summary>
        /// <param name="Xtab">Le tableau d'origine à copier.</param>
        /// <returns>Une nouvelle copie du tableau 1D.</returns>
        public static int[] CopieTab1D(int[] Xtab)
        {
            // Vérifier si le tableau d'origine est nul
            if (Xtab == null)
                throw new ArgumentNullException(nameof(Xtab), "Le tableau d'origine ne peut pas être nul.");

            // Créer une nouvelle copie du tableau 1D
            int[] resultat = new int[Xtab.Length];
            for (int i = 0; i < Xtab.Length; i++)
            {
                resultat[i] = Xtab[i];
            }

            return resultat;
        }

        /// <summary>
        /// Copie un tableau 2D.
        /// </summary>
        /// <param name="Xtab">Le tableau d'origine à copier.</param>
        /// <returns>Une nouvelle copie du tableau 2D.</returns>
        public static int[,] CopieTab2D(int[,] Xtab)
        {
            // Vérifier si le tableau d'origine est nul
            if (Xtab == null)
                throw new ArgumentNullException(nameof(Xtab), "Le tableau d'origine ne peut pas être nul.");

            // Obtenir les dimensions du tableau 2D
            int ligne = Xtab.GetLength(0);
            int cols = Xtab.GetLength(1);

            // Créer une nouvelle copie du tableau 2D
            int[,] resultat = new int[ligne, cols];
            for (int i = 0; i < ligne; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    resultat[i, j] = Xtab[i, j];
                }
            }
            return resultat;
        }

        /// <summary>
        /// Copie une colonne spécifique d'un tableau 2D.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D d'origine.</param>
        /// <param name="index">L'index de la colonne à copier.</param>
        /// <returns>
        /// Un tableau 1D représentant la colonne copiée, ou null si le tableau d'origine ou l'index de colonne sont invalides.
        /// </returns>
        public static int[] CopieColonne(int[,] Xtab, int index)
        {
            // Obtenir le nombre de lignes du tableau 2D
            int lignes = Xtab.GetLength(0);

            // Créer une nouvelle copie de la colonne
            int[] resultat = new int[lignes];
            for (int i = 0; i < lignes; i++)
            {
                resultat[i] = Xtab[i, index];
            }

            return resultat;
        }

        /// <summary>
        /// Copie une ligne spécifique d'un tableau 2D.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D d'origine.</param>
        /// <param name="index">L'index de la ligne à copier.</param>
        /// <returns>
        /// Un tableau 1D représentant la ligne copiée, ou null si l'index de ligne est invalide ou le tableau d'origine est null.
        /// </returns>
        public static int[] CopieLigne(int[,] Xtab, int index)
        {
            // Obtenir le nombre de colonnes du tableau 2D
            int colonnes = Xtab.GetLength(1);
            // Créer une nouvelle copie de la ligne
            int[] resultat = new int[colonnes];
            for (int j = 0; j < colonnes; j++)
            {
                resultat[j] = Xtab[index, j];
            }
            return resultat;
        }

        /// <summary>
        /// Propage les distances verticalement dans un tableau 2D d'entiers.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D d'entiers sur lequel effectuer la propagation.</param>
        /// <returns>Le tableau 2D résultant de la propagation des distances verticales.</returns>
        public static int[,] PropagationVerticale(int[,] Xtab)
        {
            int hauteur = Xtab.GetLength(0);
            int largeur = Xtab.GetLength(1);
            int[,] tabResultats = new int[hauteur, largeur];

            for (int i = 0; i < largeur; i++)
            {
                int[] col = CopieColonne(Xtab, i);
                PropagationVersLeBas(col);
                PropagationVersLeHaut(col);
                // Mise à jour des colonnes du tableau des propagations verticales resultant
                for (int ligne = 0; ligne < hauteur; ligne++)
                {
                    tabResultats[ligne, i] = col[ligne];
                }
            }

            return tabResultats;
        }

        /// <summary>
        /// Propage les distances horizontalement dans un tableau 2D d'entiers.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D d'entiers sur lequel effectuer la propagation.</param>
        /// <returns>Le tableau 2D résultant de la propagation des distances horizontales.</returns>
        public static int[,] PropagationHorizontale(int[,] Xtab)
        {
            int hauteur = Xtab.GetLength(0);
            int largeur = Xtab.GetLength(1);
            int[,] tabResultats = new int[hauteur, largeur];

            for (int i = 0; i < hauteur; i++)
            {
                int[] ligne = CopieLigne(Xtab, i);

                for (int ind = 0; ind < largeur; ind++)
                {
                    int distanceMini = ligne[ind];

                    for (int k = 0; k < largeur; k++)
                    {
                        int distance = Xtab[i, k] + (k - ind) * (k - ind);

                        if (distanceMini > distance)
                        {
                            distanceMini = distance;
                        }
                    }

                    ligne[ind] = distanceMini;
                }

                for (int col = 0; col < largeur; col++)
                {
                    tabResultats[i, col] = ligne[col];
                }
            }

            return tabResultats;
        }

        /// <summary>
        /// Effectue la propagation vers le bas sur un tableau 1D.
        /// </summary>
        /// <param name="col">Le tableau 1D à modifier.</param>
        public static void PropagationVersLeBas(int[] col)
        {
            int step = 1;

            for (int ind = 1; ind < col.Length; ind++)
            {
                if (col[ind] > col[ind - 1] + step)
                {
                    col[ind] = col[ind - 1] + step;
                    step += 2;
                }
                else
                {
                    step = 1;
                }
            }
        }

        /// <summary>
        /// Effectue la propagation vers le haut sur un tableau 1D.
        /// </summary>
        /// <param name="col">Le tableau 1D à modifier.</param>
        public static void PropagationVersLeHaut(int[] col)
        {
            int step = 1;

            for (int ind = col.Length - 2; ind >= 0; ind--)
            {
                if (col[ind] > col[ind + 1] + step)
                {
                    col[ind] = col[ind + 1] + step;
                    step += 2;
                }
                else
                {
                    step = 1;
                }
            }
        }

        /// <summary>
        /// Crée une liste de boules à partir d'un tableau 2D représentant une carte distance.
        /// </summary>
        /// <param name="carteDistance">La carte de distance (Tableau 2D).</param>
        /// <returns>Une liste de boules.</returns>
        public static List<Boule> CreerListeBoules(int[,] carteDistance)
        {
            List<Boule> boules = new List<Boule>();
            int hauteur = carteDistance.GetLength(0);
            int largeur = carteDistance.GetLength(1);

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if (carteDistance[i, j] != 0)
                    {
                        boules.Add(new Boule(i, j, carteDistance[i, j]));
                    }
                }
            }

            return boules;
        }
        /// <summary>
        /// Trie une liste de boules par rayon de manière décroissante.
        /// </summary>
        /// <param name="boules">La liste de boules à trier.</param>
        public static void TrierBoulesParRayonDecroissant(List<Boule> boules)
        {
            boules.Sort((a, b) => b.Rayon.CompareTo(a.Rayon));
        }
        /// <summary>
        /// Trouve les boules maximales parmi une liste de boules.
        /// </summary>
        /// <param name="boules">La liste de boules à évaluer.</param>
        /// <returns>Une liste de boules maximales.</returns>
        public static List<Boule> TrouverBoulesMaximales(List<Boule> boules)
        {
            List<Boule> boulesMax = new List<Boule>();

            foreach (Boule boule in boules)
            {
                if (EstBouleMaximale(boule, boulesMax))
                {
                    boulesMax.Add(boule);
                }
            }

            return boulesMax;
        }
        /// <summary>
        /// Vérifie si une boule est maximale parmi un ensemble de boules maximales existantes.
        /// </summary>
        /// <param name="boule">La boule à évaluer.</param>
        /// <param name="boulesMax">La liste des boules maximales existantes.</param>
        /// <returns>True si la boule est maximale, sinon False.</returns>
        public static bool EstBouleMaximale(Boule boule, List<Boule> boulesMax)
        {
            foreach (Boule bouleMax in boulesMax)
            {
                if (boule.EstIncluse(bouleMax)) return false;
            }
            return true;
        }
        /********************************************************************************************
        *********************************************************************************************
        ********************************     AFFICHAGE D'IMAGE     **********************************
        *********************************************************************************************
        *********************************************************************************************
        ********************************************************************************************/


        /// <summary>
        /// Crée un tableau 2D stockant les valeurs des pixels de l'image située au chemin spécifié.
        /// </summary>
        /// <param name="Xfile">Le chemin vers l'image BMP.</param>
        /// <returns>Le tableau 2D stockant les valeurs des pixels de l'image.</returns>
        public static int[,] TabFromFile(string Xfile)
        {
            Bitmap img = new Bitmap(Xfile);
            int[,] tabImage = ImageToInt(img);
            return tabImage;
        }

        /// <summary>
        /// Convertit une image en un tableau 2D stockant les valeurs des pixels.
        /// </summary>
        /// <param name="Ximg">L'image au format Bitmap.</param>
        /// <returns>Le tableau 2D stockant les valeurs des pixels de l'image Ximg.</returns>
        public static int[,] ImageToInt(Bitmap Ximg)
        {
            int largeur = Ximg.Width;
            int hauteur = Ximg.Height;
            int[,] tab = new int[hauteur, largeur];
            for (int lig = 0; lig < hauteur; lig++)
            {
                for (int col = 0; col < largeur; col++)
                {
                    Color c = Ximg.GetPixel(col, lig);

                    tab[lig, col] = (int)c.R;
                }
            }
            return tab;
        }

        /// <summary>
        /// Remplit Ximg à partir de Xtab.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D stockant les valeurs des pixels de l'image Ximg.</param>
        /// <param name="Ximg">L'image Bitmap résultante</param>
        public static void IntToImage(int[,] Xtab, Bitmap Ximg)
        {
            int hauteur = Xtab.GetLength(0);
            int largeur = Xtab.GetLength(1);
            int maxValue = TrouveValMax(Xtab);

            for (int lig = 0; lig < hauteur; lig++)
            {
                for (int col = 0; col < largeur; col++)
                {
                    int valeurPixel = Xtab[lig, col];
                    // Normalise la valeur du pixel dans l'intervalle [0, 255] en fonction de la valeur maximale du tableau
                    int valeurNormalise = (int)((valeurPixel / (double)maxValue) * 255.0);
                    Color c;
                    // Si la valeur du pixel est égale à 0 (noir) il devient blanc
                    if (valeurNormalise == 0) c = Color.FromArgb(255, 255, 255, 255);
                    else c = Color.FromArgb(255, valeurNormalise, valeurNormalise, valeurNormalise);
                    Ximg.SetPixel(col, lig, c);
                }
            }
        }


        /// <summary>
        /// Sauvegarde l'image dont la valeur des pixels est stockée dans Xtab, au chemin spécifié par Xfile.
        /// </summary>
        /// <param name="Xfile">Le chemin de l'image au format Bitmap.</param>
        /// <param name="Xtab">Le tableau 2D stockant les valeurs des pixels de l'image à sauvegarder.</param>
        public static void SaveImage(int[,] Xtab, string Xfile)
        {
            Bitmap img = new Bitmap(Xtab.GetLength(1), Xtab.GetLength(0));
            IntToImage(Xtab, img);
            img.Save(Xfile);
            Console.WriteLine("Saugarde dans le fichier : " + Xfile);
        }

        /// <summary>
        /// Affiche l'image dont la valeur des pixels est stockée dans Xtab.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D stockant les valeurs des pixels de l'image à afficher.</param>
        public static void Affiche_image(int[,] Xtab)
        {
            Bitmap img = new Bitmap(Xtab.GetLength(1), Xtab.GetLength(0));
            IntToImage(Xtab, img);
            Form f = new Form();
            f.BackgroundImage = img;
            f.Width = img.Width;
            f.Height = img.Height;
            f.Show();

        }

        /********************************************************************************************
        *********************************************************************************************
        *****************************     AFFICHAGE DU SQUELLETTE     *******************************
        *********************************************************************************************
        *********************************************************************************************
        ********************************************************************************************/


        /// <summary>
        /// Remplit une image par les boules maximales(CERCLES) à partir de Xtab.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D stockant les valeurs des pixels de l'image Ximg.</param>
        /// <param name="Ximg">L'image Bitmap résultante</param>

        public static void IntToSquelette(int[,] Xtab, List<Boule> XboulesMax, Bitmap Ximg)
        {
            int hauteur = Xtab.GetLength(0);
            int largeur = Xtab.GetLength(1);
            using (Graphics g = Graphics.FromImage(Ximg))
            {
                g.DrawImage(Ximg, 0, 0);

                for (int lig = 0; lig < hauteur; lig++)
                {
                    for (int col = 0; col < largeur; col++)
                    {
                        int valeurPixel = Xtab[lig, col];
                        Color finalColor = Color.FromArgb(255, 255, 255, 255); ;
                       // if (valeurPixel == 255) finalColor = Color.FromArgb(0, 0, 0, 0);
                        //else finalColor = Color.FromArgb(255, valeurPixel, valeurPixel, valeurPixel);
                        Ximg.SetPixel(col, lig, finalColor);
                    }
                }

                foreach (Boule boule in XboulesMax)
                {

                    int rayon = (int)Math.Round(Math.Sqrt(boule.Rayon));
                    Color circleGradientColor = CalculateGradientColor(boule.X, largeur);
                    // Dessin des cercles représentant les boules maximales
                    //DrawCircle(g, boule.Y, boule.X, rayon, circleGradientColor);

                    SolidBrush blackBrush = new SolidBrush(Color.Black);

                    g.FillEllipse(blackBrush, boule.Y - rayon, boule.X - rayon, 2 * rayon, 2 * rayon);

                    // Make sure to dispose of the brush to free up resources
                    blackBrush.Dispose();
                }
            }
        }
        /// <summary>
        /// Calcule une couleur de dégradé en fonction de la colonne actuelle.
        /// </summary>
        /// <param name="col">La colonne actuelle.</param>
        /// <param name="width">La largeur de l'image.</param>
        /// <returns>La couleur calculée.</returns>
        public static Color CalculateGradientColor(int col, int width)
        {
            Color color1 = Color.FromArgb(0x00DBDE);
            Color color2 = Color.FromArgb(0x635EE2);
            Color color3 = Color.FromArgb(0x9564D2);

            float t = (float)col / (float)width;

            Color interpolatedColor1 = InterpolateColors(color1, color2, t);
            Color interpolatedColor2 = InterpolateColors(color2, color3, t);

            return InterpolateColors(interpolatedColor1, interpolatedColor2, t);
        }
        /// <summary>
        /// Interpole entre deux couleurs en fonction d'un facteur t.
        /// </summary>
        /// <param name="color1">La première couleur.</param>
        /// <param name="color2">La deuxième couleur.</param>
        /// <param name="t">Le facteur d'interpolation.</param>
        /// <returns>La couleur interpolée.</returns>
        public static Color InterpolateColors(Color color1, Color color2, float t)
        {
            int r = (int)(color1.R + t * (color2.R - color1.R));
            int g = (int)(color1.G + t * (color2.G - color1.G));
            int b = (int)(color1.B + t * (color2.B - color1.B));

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Mélange deux couleurs.
        /// </summary>
        /// <param name="baseColor">La couleur de base.</param>
        /// <param name="blendColor">La couleur à mélanger.</param>
        /// <returns>La couleur résultante.</returns>
        public static Color BlendColors(Color baseColor, Color blendColor)
        {
            int alpha = blendColor.A;
            int invAlpha = 255 - alpha;
            int r = (baseColor.R * invAlpha + blendColor.R * alpha) / 255;
            int g = (baseColor.G * invAlpha + blendColor.G * alpha) / 255;
            int b = (baseColor.B * invAlpha + blendColor.B * alpha) / 255;
            return Color.FromArgb(255, r, g, b);
        }

        /// <summary>
        /// Dessine un cercle sur le graphique g aux coordonnées spécifiées.
        /// </summary>
        /// <param name="g">Le graphique sur lequel dessiner.</param>
        /// <param name="centerX">La coordonnée X du centre du cercle.</param>
        /// <param name="centerY">La coordonnée Y du centre du cercle.</param>
        /// <param name="radius">Le rayon du cercle.</param>
        /// <param name="color">La couleur du cercle.</param>
        public static void DrawCircle(Graphics g, int centerX, int centerY, int radius, Color color)
        {
            using (Pen pen = new Pen(color))
            {
                g.DrawEllipse(pen, centerX - radius, centerY - radius, 2 * radius, 2 * radius);
            }
        }

        /// <summary>
        /// Remplit l'image avec le squelette avec des points à partir de Xtab.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D stockant les valeurs des pixels de l'image Ximg.</param>
        /// <param name="Ximg">L'image Bitmap résultante</param>
        ///

        public static void IntToSquelettePoint(int[,] Xtab, List<Boule> XboulesMax, Bitmap Ximg)
        {
            int hauteur = Xtab.GetLength(0);
            int largeur = Xtab.GetLength(1);

            for (int lig = 0; lig < hauteur; lig++)
            {
                for (int col = 0; col < largeur; col++)
                {
                    int valeurPixel = Xtab[lig, col];
                    Color c;
                    if (valeurPixel == 255)
                    {
                        c = Color.FromArgb(0, 0, 0, 0);
                    }
                    else
                    {
                        c = Color.FromArgb(255, valeurPixel, valeurPixel, valeurPixel);
                        //finalColor = BlendColors(pixelColor, gradientColor);
                    }
                    Ximg.SetPixel(col, lig, c);
                }
            }
            foreach (Boule boule in XboulesMax)
            {
                Color c = Color.Purple;
                Ximg.SetPixel(boule.Y, boule.X, c);
            }
        }

        /// <summary>
        /// Sauvegarde l'image dont la valeur des pixels est stockée dans Xtab, au chemin spécifié par Xfile.
        /// </summary>
        /// <param name="Xfile">Le chemin de l'image au format Bitmap.</param>
        /// <param name="Xtab">Le tableau 2D stockant les valeurs des pixels de l'image à sauvegarder.</param>
        public static void SaveSquelette(int[,] Xtab, List<Boule> XboulesMax, string Xfile)
        {
            Bitmap img = new Bitmap(Xtab.GetLength(1), Xtab.GetLength(0));
            IntToSquelette(Xtab, XboulesMax, img);
            img.Save(Xfile);
            Console.WriteLine("Saugarde dans le fichier : " + Xfile);
        }

        /// <summary>
        /// Affiche l'image dont la valeur des pixels est stockée dans Xtab.
        /// </summary>
        /// <param name="Xtab">Le tableau 2D stockant les valeurs des pixels de l'image à afficher.</param>
        public static void Affiche_Squelette(int[,] Xtab, List<Boule> XboulesMax)
        {
            Bitmap img = new Bitmap(Xtab.GetLength(1), Xtab.GetLength(0));
            IntToSquelette(Xtab, XboulesMax, img);
            Form f = new Form();
            f.BackgroundImage = img;
            f.Width = img.Width;
            f.Height = img.Height;
            f.Show();
        }

        /********************************************************************************************
        *********************************************************************************************
        *****************************   COMPARAISON DES ALGORITHMES    ******************************
        *********************************************************************************************
        *********************************************************************************************
        ********************************************************************************************/
        /// <summary>
        /// Mesure le temps de calcul
        /// </summary>

        public static void WatchSEDTNoisePerformance(Func<int[,], int[,]> Algortihm, string AlgoType)
        {
            string[] imagePaths = {
                "../../images/imagesTheoriques/img1/image0.bmp",
                "../../images/imagesTheoriques/img1/image1.bmp",
                "../../images/imagesTheoriques/img1/image2.bmp",
                "../../images/imagesTheoriques/img1/image3.bmp",
                "../../images/imagesTheoriques/img1/image4.bmp"
            };
            for (int j = 2; j < 4; j++)
            {
                Chart chart = new Chart();
                chart.Titles.Add(new Title("Temps de calcul en fonction de bruit"));
                chart.ChartAreas.Add(new ChartArea("Temps de calcul en fonction de bruit"));


                Series series = chart.Series.Add($"SEDT {AlgoType}");
                series.ChartType = SeriesChartType.Line;
                int index = 0;
                chart.ChartAreas[0].AxisX.IsMarginVisible = false;


                foreach (string imagePath in imagePaths)
                {
                    int[,] tabImage = TabFromFile(imagePath);
                    int[,] tabInit = InitResultat(tabImage);

                    Console.WriteLine($"Image: {imagePath}");

                    Stopwatch stopwatchOptimise = new Stopwatch();
                    stopwatchOptimise.Start();
                    int[,] resultatOptimise = Algortihm(tabInit);
                    stopwatchOptimise.Stop();
                    long tempsOptimise = stopwatchOptimise.ElapsedMilliseconds;

                    Console.WriteLine($"Temps de calcul ({AlgoType}): {tempsOptimise} ms");

                    series.Points.AddXY(index, tempsOptimise);
                    index++;
                }

                chart.ChartAreas[0].AxisX.Title = "Intensité de bruit";
                chart.ChartAreas[0].AxisY.Title = "Temps de calcul (ms)";

                chart.Legends.Add(new Legend("Légende"));
                chart.Legends["Légende"].Docking = Docking.Bottom;

                Form form = new Form();
                ChartControl chartControl = new ChartControl(chart);
                chartControl.Dock = DockStyle.Fill;
                form.Controls.Add(chartControl);
                //form.ShowDialog();
                string pathToSave = $"../../images/GraphBruitSEDT{j}.png";
                chart.SaveImage(pathToSave, ChartImageFormat.Png);
                for (int i = 0; i < 5; i++)
                {
                    imagePaths[i] = imagePaths[i].Replace("img1", $"img{j}");
                }
            }
        }
        public static void WatchSEDTFormePixelPerformance(Func<int[,], int[,]> Algortihm, string AlgoType)
        {
            List<string> images = new List<string>();
            for (int i = 1; i < 20; i++)
            {
                string x = "../../images/imagesTheoriques/img1/image0.bmp";
                x = x.Replace("img1", $"img{i}");
                images.Add(x);
            }
            var imagePaths = images.OrderBy(TrieCheminImagePixelTotal);
            foreach(var i in imagePaths)
            {
                Console.WriteLine(i);
            }
            Chart chart = new Chart();
            chart.Titles.Add(new Title("Temps de calcul en fonction de nombre de pixel de la forme"));
            chart.ChartAreas.Add(new ChartArea("Temps de calcul en fonction de nombre de pixel de la forme"));


            Series series = chart.Series.Add($"SEDT {AlgoType}");
            series.ChartType = SeriesChartType.Line;
            int index = 0;
            chart.ChartAreas[0].AxisX.IsMarginVisible = false;


            foreach (string imagePath in imagePaths)
            {
                int[,] tabImage = TabFromFile(imagePath);
                int[,] tabInit = InitResultat(tabImage);
                int nombreTotalPixels = tabImage.GetLength(0) * tabImage.GetLength(1) / 10000;

                Console.WriteLine($"Image: {imagePath}");
                //int nombrePixelForme = GetFormPixels(tabInit);
                Stopwatch stopwatchOptimise = new Stopwatch();
                stopwatchOptimise.Start();
                int[,] resultatOptimise = Algortihm(tabInit);
                stopwatchOptimise.Stop();
                long tempsOptimise = stopwatchOptimise.ElapsedMilliseconds;

                Console.WriteLine($"Temps de calcul ({AlgoType}): {tempsOptimise} ms");

                series.Points.AddXY(nombreTotalPixels, tempsOptimise);
                index++;
            }

            chart.ChartAreas[0].AxisX.Title = "Nombre de pixels de la forme";
            chart.ChartAreas[0].AxisY.Title = "Temps de calcul (ms)";

            chart.Legends.Add(new Legend("Légende"));
            chart.Legends["Légende"].Docking = Docking.Bottom;

            Form form = new Form();
            ChartControl chartControl = new ChartControl(chart);
            chartControl.Dock = DockStyle.Fill;
            form.Controls.Add(chartControl);
            //form.ShowDialog();
            string pathToSave = $"../../images/GraphFormeSEDT{AlgoType}.png";
            chart.SaveImage(pathToSave, ChartImageFormat.Png);
        }
        public static void WatchBouleMaxNoisePerformance(Func<int[,], List<Boule>> Algortihm, string AlgoType)
        {
            string[] imagePaths = {
                "../../images/imagesTheoriques/img1/image0.bmp",
                "../../images/imagesTheoriques/img1/image1.bmp",
                "../../images/imagesTheoriques/img1/image2.bmp",
                "../../images/imagesTheoriques/img1/image3.bmp",
                "../../images/imagesTheoriques/img1/image4.bmp"
            };
            for (int j = 2; j < 4; j++)
            {
                Chart chart = new Chart();
                chart.Titles.Add(new Title("Temps de calcul en fonction de bruit"));
                chart.ChartAreas.Add(new ChartArea("Temps de calcul en fonction de bruit"));


                Series series = chart.Series.Add($"Boules Max {AlgoType}");
                series.ChartType = SeriesChartType.Line;
                int index = 0;
                chart.ChartAreas[0].AxisX.IsMarginVisible = false;


                foreach (string imagePath in imagePaths)
                {
                    int[,] tabImage = TabFromFile(imagePath);
                    int[,] tabInit = InitResultat(tabImage);

                    Console.WriteLine($"Image: {imagePath}");

                    Stopwatch stopwatchOptimise = new Stopwatch();
                    int[,] resultatOptimise = CarteOptimise(tabInit);
                    stopwatchOptimise.Start();
                    List<Boule> boules = Algortihm(resultatOptimise);
                    stopwatchOptimise.Stop();
                    long tempsOptimise = stopwatchOptimise.ElapsedMilliseconds;

                    Console.WriteLine($"Temps de calcul ({AlgoType}): {tempsOptimise} ms");

                    series.Points.AddXY(index, tempsOptimise);
                    index++;
                }

                chart.ChartAreas[0].AxisX.Title = "Intensité de bruit";
                chart.ChartAreas[0].AxisY.Title = "Temps de calcul (ms)";

                chart.Legends.Add(new Legend("Légende"));
                chart.Legends["Légende"].Docking = Docking.Bottom;

                Form form = new Form();
                ChartControl chartControl = new ChartControl(chart);
                chartControl.Dock = DockStyle.Fill;
                form.Controls.Add(chartControl);
                //form.ShowDialog();
                string pathToSave = $"../../images/GraphBruitBouleMax{j}.png";
                chart.SaveImage(pathToSave, ChartImageFormat.Png);
                for (int i = 0; i < 5; i++)
                {
                    imagePaths[i] = imagePaths[i].Replace("img1", $"img{j}");
                }
            }
        }
        public static void WatchBouleMaxFormePixelPerformance(Func<int[,], List<Boule>> Algortihm, string AlgoType)
        {
            List<string> images = new List<string> ();
            for (int i = 1; i < 8; i++)
            {
                string x = "../../images/imagesTheoriques/img1/image0.bmp";
                x = x.Replace("img1", $"img{i}");
                images.Add(x);
            }
            var imagePaths = images.OrderBy(TrieCheminImagePixelFond);
            Chart chart = new Chart();
            chart.Titles.Add(new Title("Temps de calcul en fonction de nombre de pixel de la forme"));
            chart.ChartAreas.Add(new ChartArea("Temps de calcul en fonction de nombre de pixel de la forme"));


            Series series = chart.Series.Add($"Boules Max {AlgoType}");
            series.ChartType = SeriesChartType.Line;
            int index = 0;
            chart.ChartAreas[0].AxisX.IsMarginVisible = false;


            foreach (string imagePath in imagePaths)
            {
                int[,] tabImage = TabFromFile(imagePath);
                int[,] tabInit = InitResultat(tabImage);
                
                Console.WriteLine($"Image: {imagePath}");
                int nombrePixelForme = GetFormPixels(tabInit);
                Stopwatch stopwatchOptimise = new Stopwatch();
                int[,] resultatOptimise = CarteOptimise(tabInit);
                stopwatchOptimise.Start();
                List<Boule> boules = Algortihm(resultatOptimise);
                stopwatchOptimise.Stop();
                long tempsOptimise = stopwatchOptimise.ElapsedMilliseconds;

                Console.WriteLine($"Temps de calcul ({AlgoType}): {tempsOptimise} ms");

                series.Points.AddXY(nombrePixelForme, tempsOptimise);
                index++;
            }

            chart.ChartAreas[0].AxisX.Title = "Nombre de pixels de la forme";
            chart.ChartAreas[0].AxisY.Title = "Temps de calcul (ms)";

            chart.Legends.Add(new Legend("Légende"));
            chart.Legends["Légende"].Docking = Docking.Bottom;

            Form form = new Form();
            ChartControl chartControl = new ChartControl(chart);
            chartControl.Dock = DockStyle.Fill;
            form.Controls.Add(chartControl);
            //form.ShowDialog();
            string pathToSave = $"../../images/GraphFormeBouleMax{AlgoType}.png";
            chart.SaveImage(pathToSave, ChartImageFormat.Png);            
        }

    }
}

using System.Globalization;
using Microsoft.Maui.Controls;

namespace PlatReserve.Converters
{
    public class SeatColorConverter : IValueConverter
    {
        // Cette méthode transforme la donnée (chiffre) en couleur
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int placesRestantes)
            {
                if (placesRestantes <= 0)
                    return Colors.Red;       // Plus de place -> Rouge

                if (placesRestantes < 10)
                    return Colors.Orange;    // Presque plein -> Orange

                return Colors.Green;         // Beaucoup de place -> Vert
            }

            return Colors.Black; // Par défaut
        }

        // On ne l'utilise presque jamais, mais elle est obligatoire pour l'interface
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


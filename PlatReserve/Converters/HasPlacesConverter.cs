using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;


namespace PlatReserve.Converters
{
    public class HasPlacesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // On vérifie si la valeur reçue est bien un nombre
            if (value is int places)
            {
                // Si places est supérieur à 0, on retourne true (activé)
                // Sinon on retourne false (désactivé/grisé)
                return places > 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


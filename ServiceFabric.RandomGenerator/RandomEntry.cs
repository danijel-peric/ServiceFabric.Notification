using System;

namespace ServiceFabric.RandomGenerator
{
    public class RandomEntry : IEquatable<RandomEntry>
    {
        public string name { get; set; }
        public string address { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string maiden_name { get; set; }
        public string birth_data { get; set; }
        public string phone_h { get; set; }
        public string phone_w { get; set; }
        public string email_u { get; set; }
        public string email_d { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string domain { get; set; }
        public string useragent { get; set; }
        public string ipv4 { get; set; }
        public string macaddress { get; set; }
        public string plasticcard { get; set; }
        public string cardexpir { get; set; }
        public int bonus { get; set; }
        public string company { get; set; }
        public string color { get; set; }
        public string uuid { get; set; }
        public int height { get; set; }
        public double weight { get; set; }
        public string blood { get; set; }
        public string eye { get; set; }
        public string hair { get; set; }
        public string pict { get; set; }
        public string url { get; set; }
        public string sport { get; set; }
        public string ipv4_url { get; set; }
        public string email_url { get; set; }
        public string domain_url { get; set; }

        public bool Equals(RandomEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(name, other.name, StringComparison.OrdinalIgnoreCase) && string.Equals(address, other.address, StringComparison.OrdinalIgnoreCase) && latitude.Equals(other.latitude) && longitude.Equals(other.longitude) && string.Equals(maiden_name, other.maiden_name, StringComparison.OrdinalIgnoreCase) && string.Equals(birth_data, other.birth_data, StringComparison.OrdinalIgnoreCase) && string.Equals(phone_h, other.phone_h, StringComparison.OrdinalIgnoreCase) && string.Equals(phone_w, other.phone_w, StringComparison.OrdinalIgnoreCase) && string.Equals(email_u, other.email_u, StringComparison.OrdinalIgnoreCase) && string.Equals(email_d, other.email_d, StringComparison.OrdinalIgnoreCase) && string.Equals(username, other.username, StringComparison.OrdinalIgnoreCase) && string.Equals(password, other.password, StringComparison.OrdinalIgnoreCase) && string.Equals(domain, other.domain, StringComparison.OrdinalIgnoreCase) && string.Equals(useragent, other.useragent, StringComparison.OrdinalIgnoreCase) && string.Equals(ipv4, other.ipv4, StringComparison.OrdinalIgnoreCase) && string.Equals(macaddress, other.macaddress, StringComparison.OrdinalIgnoreCase) && string.Equals(plasticcard, other.plasticcard, StringComparison.OrdinalIgnoreCase) && string.Equals(cardexpir, other.cardexpir, StringComparison.OrdinalIgnoreCase) && bonus == other.bonus && string.Equals(company, other.company, StringComparison.OrdinalIgnoreCase) && string.Equals(color, other.color, StringComparison.OrdinalIgnoreCase) && string.Equals(uuid, other.uuid, StringComparison.OrdinalIgnoreCase) && height == other.height && weight.Equals(other.weight) && string.Equals(blood, other.blood, StringComparison.OrdinalIgnoreCase) && string.Equals(eye, other.eye, StringComparison.OrdinalIgnoreCase) && string.Equals(hair, other.hair, StringComparison.OrdinalIgnoreCase) && string.Equals(pict, other.pict, StringComparison.OrdinalIgnoreCase) && string.Equals(url, other.url, StringComparison.OrdinalIgnoreCase) && string.Equals(sport, other.sport, StringComparison.OrdinalIgnoreCase) && string.Equals(ipv4_url, other.ipv4_url, StringComparison.OrdinalIgnoreCase) && string.Equals(email_url, other.email_url, StringComparison.OrdinalIgnoreCase) && string.Equals(domain_url, other.domain_url, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RandomEntry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(name) : 0);
                hashCode = (hashCode * 397) ^ (address != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(address) : 0);
                hashCode = (hashCode * 397) ^ latitude.GetHashCode();
                hashCode = (hashCode * 397) ^ longitude.GetHashCode();
                hashCode = (hashCode * 397) ^ (maiden_name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(maiden_name) : 0);
                hashCode = (hashCode * 397) ^ (birth_data != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(birth_data) : 0);
                hashCode = (hashCode * 397) ^ (phone_h != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(phone_h) : 0);
                hashCode = (hashCode * 397) ^ (phone_w != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(phone_w) : 0);
                hashCode = (hashCode * 397) ^ (email_u != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(email_u) : 0);
                hashCode = (hashCode * 397) ^ (email_d != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(email_d) : 0);
                hashCode = (hashCode * 397) ^ (username != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(username) : 0);
                hashCode = (hashCode * 397) ^ (password != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(password) : 0);
                hashCode = (hashCode * 397) ^ (domain != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(domain) : 0);
                hashCode = (hashCode * 397) ^ (useragent != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(useragent) : 0);
                hashCode = (hashCode * 397) ^ (ipv4 != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ipv4) : 0);
                hashCode = (hashCode * 397) ^ (macaddress != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(macaddress) : 0);
                hashCode = (hashCode * 397) ^ (plasticcard != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(plasticcard) : 0);
                hashCode = (hashCode * 397) ^ (cardexpir != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(cardexpir) : 0);
                hashCode = (hashCode * 397) ^ bonus;
                hashCode = (hashCode * 397) ^ (company != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(company) : 0);
                hashCode = (hashCode * 397) ^ (color != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(color) : 0);
                hashCode = (hashCode * 397) ^ (uuid != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(uuid) : 0);
                hashCode = (hashCode * 397) ^ height;
                hashCode = (hashCode * 397) ^ weight.GetHashCode();
                hashCode = (hashCode * 397) ^ (blood != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(blood) : 0);
                hashCode = (hashCode * 397) ^ (eye != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(eye) : 0);
                hashCode = (hashCode * 397) ^ (hair != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(hair) : 0);
                hashCode = (hashCode * 397) ^ (pict != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(pict) : 0);
                hashCode = (hashCode * 397) ^ (url != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(url) : 0);
                hashCode = (hashCode * 397) ^ (sport != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(sport) : 0);
                hashCode = (hashCode * 397) ^ (ipv4_url != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ipv4_url) : 0);
                hashCode = (hashCode * 397) ^ (email_url != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(email_url) : 0);
                hashCode = (hashCode * 397) ^ (domain_url != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(domain_url) : 0);
                return hashCode;
            }
        }

        public static bool operator ==(RandomEntry left, RandomEntry right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RandomEntry left, RandomEntry right)
        {
            return !Equals(left, right);
        }
    }
}

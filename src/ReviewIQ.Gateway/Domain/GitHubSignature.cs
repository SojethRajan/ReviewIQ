namespace ReviewIQ.Gateway.Domain
{
    public record GitHubSignature
    {
        public string Value { get; }

        private GitHubSignature(string value)
        {
            Value = value;
        }

        public static GitHubSignature Parse(string signatureHeader)
        {
            if (string.IsNullOrWhiteSpace(signatureHeader))
            {
                throw new ArgumentException("Signature header cannot be null or empty.", nameof(signatureHeader));
            }

            if (!signatureHeader.StartsWith("sha256="))
            {
                throw new ArgumentException("Signature must start with sha256=.");
            }

            return new GitHubSignature(signatureHeader);
        }

        public string Hash => Value.Replace("sha256=", string.Empty);
    }
}

namespace CardSystem
{
    public class _Card
    {
        public CardType card;
        public bool isMatched;

        public _Card(CardType card)
        {
            this.card = card;
            this.isMatched = false;
        }
    }
}

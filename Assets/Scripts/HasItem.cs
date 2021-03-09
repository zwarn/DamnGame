public interface HasItem
{
    bool HasItem();

    ItemType getItemType();

    void PickupItem(Item item);

    Item GiveItem();
}
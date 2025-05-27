using CloudSync.Models;
namespace CloudSync.Pages;

public partial class CorouselPage : ContentPage
{
	public CorouselPage()
	{
		InitializeComponent();
		var item = new List<CollectionList>
		{
			new CollectionList(){ Title="hello1",Description="bye1"},
            new CollectionList(){ Title="hello2",Description="bye2"},
            new CollectionList(){ Title="hello3",Description="bye3"},
            new CollectionList(){ Title="hello4",Description="bye4"}
		};
		demoCarousel.ItemsSource = item;	
	}
}
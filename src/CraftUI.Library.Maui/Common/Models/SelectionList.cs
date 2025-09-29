using System.Collections;
using System.Collections.Specialized;
using CraftUI.Library.Maui.Controls;

namespace CraftUI.Library.Maui.Common.Models;

internal class SelectionList : IList<DisplayValueItem>
{
	private static readonly IList<DisplayValueItem> SEmpty = new List<DisplayValueItem>(0);
	private readonly CfPickerMultipleSelection _selectableItemsView;
	private readonly IList<DisplayValueItem> _internal;
	private IList<DisplayValueItem> _shadow;
	private bool _externalChange;

	public SelectionList(CfPickerMultipleSelection selectableItemsView, IList<DisplayValueItem>? items = null)
	{
		_selectableItemsView = selectableItemsView ?? throw new ArgumentNullException(nameof(selectableItemsView));
		_internal = items ?? new List<DisplayValueItem>();
		_shadow = Copy();

		if (items is INotifyCollectionChanged incc)
		{
			incc.CollectionChanged += OnCollectionChanged;
		}
	}

	public DisplayValueItem this[int index] { get => _internal[index]; set => _internal[index] = value; }

	public int Count => _internal.Count;

	public bool IsReadOnly => false;

	public void Add(DisplayValueItem item)
	{
		_externalChange = true;
		_internal.Add(item);
		_externalChange = false;

		_selectableItemsView.SelectedItemsPropertyChanged(_shadow, _internal);
		_shadow.Add(item);
	}

	public void Clear()
	{
		_externalChange = true;
		_internal.Clear();
		_externalChange = false;

		_selectableItemsView.SelectedItemsPropertyChanged(_shadow, SEmpty);
		_shadow.Clear();
	}

	public bool Contains(DisplayValueItem item)
	{
		return _internal.Contains(item);
	}

	public void CopyTo(DisplayValueItem[] array, int arrayIndex)
	{
		_internal.CopyTo(array, arrayIndex);
	}

	public IEnumerator<DisplayValueItem> GetEnumerator()
	{
		return _internal.GetEnumerator();
	}

	public int IndexOf(DisplayValueItem item)
	{
		return _internal.IndexOf(item);
	}

	public void Insert(int index, DisplayValueItem item)
	{
		_externalChange = true;
		_internal.Insert(index, item);
		_externalChange = false;

		_selectableItemsView.SelectedItemsPropertyChanged(_shadow, _internal);
		_shadow.Insert(index, item);
	}

	public bool Remove(DisplayValueItem item)
	{
		_externalChange = true;
		var removed = _internal.Remove(item);
		_externalChange = false;

		if (removed)
		{
			_selectableItemsView.SelectedItemsPropertyChanged(_shadow, _internal);
			_shadow.Remove(item);
		}

		return removed;
	}

	public void RemoveAt(int index)
	{
		_externalChange = true;
		_internal.RemoveAt(index);
		_externalChange = false;

		_selectableItemsView.SelectedItemsPropertyChanged(_shadow, _internal);
		_shadow.RemoveAt(index);
	}

	List<DisplayValueItem> Copy()
	{
		var items = new List<DisplayValueItem>();
		for (int n = 0; n < _internal.Count; n++)
		{
			items.Add(_internal[n]);
		}

		return items;
	}

	private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
	{
		if (_externalChange)
		{
			// If this change was initiated by a renderer or direct manipulation of CollectionView.SelectedItems,
			// we don't need to send a selection change notification
			return;
		}

		// This change is coming from a bound viewmodel property
		// Emit a selection change notification, then bring the shadow copy up-to-date
		_selectableItemsView.SelectedItemsPropertyChanged(_shadow, _internal);
		_shadow = Copy();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
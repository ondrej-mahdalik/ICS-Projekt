using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using RideSharing.App.ViewModels;
using RideSharing.BL.Models;

namespace RideSharing.App.Wrappers;

public abstract class ModelWrapper<T> : ViewModelBase, IModel, IValidatableObject where T : IModel
{
    protected ModelWrapper(T? model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        ThisModel = model;
    }

    public Guid Id
    {
        get => GetValue<Guid>();
        set => SetValue(value);
    }

    public T ThisModel { get; }

    protected TValue? GetValue<TValue>([CallerMemberName] string? propertyName = null)
    {
        var propertyInfo = ThisModel.GetType().GetProperty(propertyName ?? string.Empty);
        return (propertyInfo?.GetValue(ThisModel) is TValue
            ? (TValue?)propertyInfo.GetValue(ThisModel)
            : default);
    }

    protected void SetValue<TValue>(TValue value, [CallerMemberName] string? propertyName = null)
    {
        var propertyInfo = ThisModel.GetType().GetProperty(propertyName ?? string.Empty);
        var currentValue = propertyInfo?.GetValue(ThisModel);
        if (!Equals(currentValue, value))
        {
            propertyInfo?.SetValue(ThisModel, value);
            OnPropertyChanged(propertyName);
        }
    }

    protected void RegisterCollection<TWrapper, TModel>(
        ObservableCollection<TWrapper> wrapperCollection,
        ICollection<TModel> modelCollection)
        where TWrapper : ModelWrapper<TModel>, IModel
        where TModel : IModel
    {
        wrapperCollection.CollectionChanged += (s, e) =>
        {
            modelCollection.Clear();
            foreach (var model in wrapperCollection.Select(i => i.ThisModel))
            {
                modelCollection.Add(model);
            }
        };
    }

    public bool IsValid => !Validate(new ValidationContext(this)).Any();

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        yield break;
    }
}

using PizzaStore.Models;

namespace PizzaStore.Services;

public static class PizzaService
{
    static int nextId = 1;

    static List<Pizza> Pizzas { get; }

    static PizzaService()
    {
        Pizzas = new List<Pizza>
        {
            new Pizza
            {
                Id = nextId++,
                Name = "Classic Italian",
                Description = "Fresh tomatoes, mozzarella, basil, and olive oil on a traditional thin crust.",
                IsGlutenFree = false
            },
            new Pizza
            {
                Id = nextId++,
                Name = "The Carnivore",
                Description = "Loaded with pepperoni, sausage, bacon, and ham on a thick crust with extra cheese.",
                IsGlutenFree = false
            },
            new Pizza
            {
                Id = nextId++,
                Name = "Garden Fresh",
                Description = "Bell peppers, mushrooms, onions, olives, and spinach on a gluten-free crust.",
                IsGlutenFree = true
            },
            new Pizza
            {
                Id = nextId++,
                Name = "BBQ Chicken",
                Description = "Grilled chicken, red onions, cilantro, and smoked gouda on a tangy BBQ sauce base.",
                IsGlutenFree = false
            },
            // Additional pizza added per assignment requirements
            new Pizza
            {
                Id = nextId++,
                Name = "Hawaiian Luau",
                Description = "Canadian bacon, fresh pineapple chunks, jalapeños, and a blend of mozzarella and cheddar on a honey-glazed crust.",
                IsGlutenFree = false
            }
        };
    }

    public static List<Pizza> GetAll() => Pizzas;

    public static Pizza? Get(int id) => Pizzas.FirstOrDefault(p => p.Id == id);

    public static Pizza Create(Pizza newPizza)
    {
        newPizza.Id = nextId++;
        Pizzas.Add(newPizza);
        return newPizza;
    }

    public static void Update(Pizza updatedPizza)
    {
        var index = Pizzas.FindIndex(p => p.Id == updatedPizza.Id);
        if (index == -1)
            return;

        Pizzas[index] = updatedPizza;
    }

    public static void Delete(int id)
    {
        var pizza = Get(id);
        if (pizza is null)
            return;

        Pizzas.Remove(pizza);
    }
}


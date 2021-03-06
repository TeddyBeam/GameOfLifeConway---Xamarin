﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GameOfLifeConway
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            var pattern = new RandomPattern();
            var grid = new RepeatableBorderLifeGrid(pattern);

            Content = grid;
        }
	}
}

using System;
using System.Numerics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ImGuiNET;
using MonoGame.Extended;

using Vector2 = System.Numerics.Vector2;
using Vector4 = System.Numerics.Vector4;


namespace ProjectAlpha2
{
	public static class UI
	{
		private static int count;
		public static void LoadContent()
		{
		}


		public static void Draw()
		{
			DrawOverviewWindow();
			DrawVisitedLocationOverview();
			DrawTime();
		}

		private static void DrawTime()
		{
			float padding = 10;

			ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.AlwaysAutoResize |
											ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing |
											ImGuiWindowFlags.NoNav;

			ImGui.SetNextWindowPos(new Vector2(padding, padding), ImGuiCond.Always, new Vector2());
			ImGui.SetNextWindowBgAlpha(0.35f);

			ImGui.Begin("Selection", window_flags);
			
			ImGui.Text(World.Time.ToString());

			ImGui.End();
		}

		private static unsafe void DrawVisitedLocationOverview()
		{
			Traveller t = World.PosessedTraveller;
			if (t.CurrentLocation != null)
			{
				ImGui.Begin("Location");
				ImGui.BeginTabBar("loc_select", ImGuiTabBarFlags.None);

				if (ImGui.BeginTabItem(t.CurrentLocation.Name + $"({t.Name})"))
				{
					ImGui.BeginChild("child1", new Vector2(), false, ImGuiWindowFlags.None);

					foreach (KeyValuePair<ItemId, List<Item>> itemListKeypair in t.CurrentLocation.Inventory.ItemLists)
					{
						if (itemListKeypair.Value.Count > 1)
						{
							bool treeOpened = ImGui.TreeNodeEx($"    \n\n\n##{itemListKeypair.Key}", ImGuiTreeNodeFlags.SpanAvailWidth);

							if (ImGui.BeginPopupContextItem($"{itemListKeypair.Key}"))
							{
								if (ImGui.IsWindowAppearing()) count = itemListKeypair.Value.Count;

								ImGui.PushItemWidth(100);
								ImGui.SliderInt("##slider", ref count, 1, itemListKeypair.Value.Count);

								ImGui.InputInt("##int", ref count);
								ImGui.PopItemWidth();
								if (count > itemListKeypair.Value.Count) count = itemListKeypair.Value.Count;
								if (count < 1) count = 1;


								if (ImGui.Button("Take", new Vector2(100, 20)))
								{
									for (int i = 0; i < count; i++)
									{
										Storage.MoveItem(itemListKeypair.Value[0], t.CurrentLocation.Inventory, t.Inventory);
									}
								}
								if (ImGui.Button("Throw out", new Vector2(100, 20)))
								{
									for (int i = 0; i < count; i++)
									{
										t.CurrentLocation.Inventory.RemoveItem(itemListKeypair.Value[0]);
									}
								}
								ImGui.EndPopup();
							}





							// if (ImGui.BeginPopupContextItem($"##{itemListKeypair.Key}"))
							// {
							//     if (ImGui.Button("Pick all"))
							//     {
							//         Storage.MoveItemSet(itemListKeypair.Key, t.CurrentLocation.Inventory, t.Inventory);
							//     }
							//     ImGui.EndPopup();
							// }
							ImGui.SameLine();
							ImGui.SetCursorPosX(ImGui.GetCursorPosX() - 35.0f);
							ImGui.Image(ResourceManager.GetTextureBinding("unknown").Item1, new Vector2(38, 38));
							ImGui.SameLine();
							ImGui.Text($"{Item.GetNameById(itemListKeypair.Key)}\n{itemListKeypair.Value.Count}");

							if (treeOpened)
							{
								foreach (Item item in itemListKeypair.Value)
								{
									ImGui.Selectable($"{item.GetName()}");

									if (ImGui.BeginPopupContextItem($"{item.GetHashCode()}"))
									{
										if (ImGui.Button("Take", new Vector2(100, 20)))
										{
											Storage.MoveItem(item, t.CurrentLocation.Inventory, t.Inventory);
											ImGui.EndPopup();
											break;
										}
										if (ImGui.Button("Throw out", new Vector2(100, 20)))
										{
											t.CurrentLocation.Inventory.RemoveItem(item);
											ImGui.EndPopup();
											break;
										}
										ImGui.EndPopup();
									}

								}
								ImGui.TreePop();
							}
						}
						else
						{
							ImGui.Selectable($"\n\n\n##{itemListKeypair.Key}");
							ImGui.SameLine();
							ImGui.BeginGroup();
							ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 14.0f);
							ImGui.Image(ResourceManager.GetTextureBinding("unknown").Item1, new Vector2(38, 38));
							ImGui.SameLine();
							ImGui.Text($"{Item.GetNameById(itemListKeypair.Key)}\n{itemListKeypair.Value.Count}");
							ImGui.SetCursorPosX(ImGui.GetWindowWidth());
							ImGui.EndGroup();


							if (ImGui.BeginPopupContextItem($"##{itemListKeypair.Key}"))
							{
								if (ImGui.Button("Take", new Vector2(100, 20)))
								{
									Storage.MoveItemSet(itemListKeypair.Key, t.CurrentLocation.Inventory, t.Inventory);
								}
								if (ImGui.Button("Throw out", new Vector2(100, 20)))
								{
									t.CurrentLocation.Inventory.RemoveItem(itemListKeypair.Value[0]);
								}

								ImGui.EndPopup();
							}

						}

						ImGui.Separator();
					}

					ImGui.EndChild();

					ImGui.EndTabItem();
				}

				ImGui.EndTabBar();
				ImGui.End();
			}

		}


		private static unsafe void DrawOverviewWindow()
		{
			Traveller t = World.PosessedTraveller;

			ImGui.Begin("Travellers");

			ImGui.BeginTabBar("trav_select", ImGuiTabBarFlags.None);

			if (ImGui.BeginTabItem(t.Name))
			{
				ImGui.EndTabItem();
				ImGui.BeginTabBar("personal", ImGuiTabBarFlags.None);
				if (ImGui.BeginTabItem("Inventory"))
				{
					ImGui.BeginChild("child1", new Vector2(), false, ImGuiWindowFlags.None);

					foreach (KeyValuePair<ItemId, List<Item>> itemListKeypair in t.Inventory.ItemLists)
					{
						if (itemListKeypair.Value.Count > 1)
						{
							bool treeOpened = ImGui.TreeNodeEx($"    \n\n\n##{itemListKeypair.Key}", ImGuiTreeNodeFlags.SpanAvailWidth);
							if (ImGui.BeginPopupContextItem($"{itemListKeypair.Key}"))
							{
								if (ImGui.IsWindowAppearing()) count = itemListKeypair.Value.Count;

								ImGui.PushItemWidth(100);
								ImGui.SliderInt("##slider", ref count, 1, itemListKeypair.Value.Count);

								ImGui.InputInt("##int", ref count);
								ImGui.PopItemWidth();
								if (count > itemListKeypair.Value.Count) count = itemListKeypair.Value.Count;
								if (count < 1) count = 1;

								if (t.CurrentLocation != null)
								{
									if (ImGui.Button($"Drop({t.CurrentLocation.Name})", new Vector2(100, 20))) 
									{
										for (int i = 0; i < count; i++)
										{
											Storage.MoveItem(itemListKeypair.Value[0], t.Inventory, t.CurrentLocation.Inventory);
										}
									}
								}
								if (ImGui.Button("Throw out", new Vector2(100, 20)))
								{
									for (int i = 0; i < count; i++)
									{
										t.Inventory.RemoveItem(itemListKeypair.Value[0]);
									}
								}
								ImGui.EndPopup();
							}
								
							ImGui.SameLine();
							ImGui.SetCursorPosX(ImGui.GetCursorPosX() - 35.0f);
							ImGui.Image(ResourceManager.GetTextureBinding("unknown").Item1, new Vector2(38, 38));
							ImGui.SameLine();
							ImGui.Text($"{Item.GetNameById(itemListKeypair.Key)}\n{itemListKeypair.Value.Count}");

							if (treeOpened)
							{
								foreach (Item item in itemListKeypair.Value)
								{
									ImGui.Selectable($"{item.GetName()}");

									if (ImGui.BeginPopupContextItem($"{item.GetHashCode()}"))
									{
										if (t.CurrentLocation != null)
										{
											if (ImGui.Button($"Drop({t.CurrentLocation.Name})", new Vector2(100, 20)))
											{
												Storage.MoveItem(item, t.Inventory, t.CurrentLocation.Inventory);
												break;
											}
										}
										if (ImGui.Button("Throw out", new Vector2(100, 20)))
										{
											t.Inventory.RemoveItem(item);
											break;
										}

										ImGui.EndPopup();
									}
								}

								ImGui.TreePop();
							}
						}
						else
						{
							ImGui.Selectable($"\n\n\n##{itemListKeypair.Key}");
							ImGui.SameLine();
							ImGui.BeginGroup();
							ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 14.0f);
							ImGui.Image(ResourceManager.GetTextureBinding("unknown").Item1, new Vector2(38, 38));
							ImGui.SameLine();
							ImGui.Text($"{Item.GetNameById(itemListKeypair.Key)}\n{itemListKeypair.Value.Count}");
							ImGui.SetCursorPosX(ImGui.GetWindowWidth());
							ImGui.EndGroup();


							if (ImGui.BeginPopupContextItem($"##{itemListKeypair.Key}"))
							{
								if (t.CurrentLocation != null)
								{
									if (ImGui.Button($"Drop({t.CurrentLocation.Name})", new Vector2(100, 20))) Storage.MoveItemSet(itemListKeypair.Key, t.Inventory, t.CurrentLocation.Inventory);
								}
								if (ImGui.Button("Throw out", new Vector2(100, 20))) t.Inventory.RemoveItemSet(itemListKeypair.Key);

								ImGui.EndPopup();
							}
						}

						ImGui.Separator();
					}

					ImGui.EndChild();

					ImGui.EndTabItem();
				}
				if (ImGui.BeginTabItem("Stats"))
				{

					ImGui.EndTabItem();
				}
				if (ImGui.BeginTabItem("Skills"))
				{

					ImGui.EndTabItem();
				}
				ImGui.EndTabBar();
			}

			ImGui.End();
		}

		// private static unsafe void DrawSelectedTravellersOverlay()
		// {
		//     if (World.GetSelectedTravellers().Count > 0)
		//     {
		//         float padding = 10;

		//         ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.AlwaysAutoResize |
		//                                         ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing |
		//                                         ImGuiWindowFlags.NoNav;

		//         ImGui.SetNextWindowPos(new Vector2(padding, padding), ImGuiCond.Always, new Vector2());
		//         ImGui.SetNextWindowBgAlpha(0.35f);

		//         ImGui.Begin("Selection", window_flags);
		//         foreach (var selTrav in World.GetSelectedTravellers())
		//         {
		//             ImGui.Text(selTrav.Name);
		//         }

		//         ImGui.End();
		//     }
		// }

		// private static unsafe void DrawPosessedTravellersList()
		// {
		//     if (World.GetPlayerPossessedTravellers().Count > 0)
		//     {
		//         float padding = 10;

		//         ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.AlwaysAutoResize |
		//                                         ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing |
		//                                         ImGuiWindowFlags.NoNav;

		//         ImGuiViewportPtr viewport = ImGui.GetMainViewport();
		//         Vector2 work_size = viewport.WorkSize;

		//         ImGui.SetNextWindowPos(new Vector2(padding, work_size.Y - padding), ImGuiCond.Always, new Vector2(0f, 1f));
		//         ImGui.SetNextWindowBgAlpha(0.35f);

		//         ImGui.Begin("Posessed travellers panel", window_flags);
		//         foreach (var posTrav in World.GetPlayerPossessedTravellers())
		//         {
		//             ImGui.BeginGroup();

		//             if (ImGui.ImageButton(ResourceManager.GetTextureBinding(posTrav.AvatarTextureId).Item1, (posTrav.Selected)? new Vector2(55, 55) : new Vector2(50, 50), 
		//                             new Vector2(), new Vector2(1, 1), 0, 
		//                             (posTrav.Selected)? new Vector4(0.2f, 0.2f, 0.8f, 0.5f) : Vector4.Zero, // button color
		//                             Vector4.One)) // foreground image color
		//             {

		//                 if (!InputManager.GetKeyboardState().IsControlDown())
		//                 {
		//                     World.UnselectAll();
		//                     posTrav.Selected = true;
		//                 }
		//                 else
		//                     posTrav.Selected = true;

		//             }
		//             ImGui.Text(posTrav.Name);
		//             ImGui.EndGroup();

		//             ImGui.SameLine();
		//         }

		//         ImGui.End();
		//     }
		// }
	}
}
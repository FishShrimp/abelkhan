﻿using System;
using System.Collections;

namespace common
{
	public class modulemanager
	{
		public modulemanager()
		{
			modules = new Hashtable();
		}

		public void add_module(String module_name, imodule _module)
		{
			modules.Add(module_name, _module);
		}

		public void process_module_mothed(String module_name, String func_name, String argvs)
		{
			if (modules.ContainsKey(module_name))
			{
				imodule _module = (imodule)modules[module_name];

				var _type = _module.GetType();
				var method = _type.GetMethod(func_name);
				if (method != null)
				{
					try
					{
						var argv = (ArrayList)System.Text.Json.Jsonparser.unpack(argvs);

						if (argv.Count > 0)
						{
							Console.WriteLine("in to process_module_mothed");

							method.Invoke(_module, argv.ToArray());
						}
						else
						{
							Console.WriteLine("in to process_module_mothed1");

							method.Invoke(_module, null);
						}
					}
					catch (Exception e)
					{
						Console.WriteLine("call rpc error {0}", e);
					}
				}
			}
			else
			{
				Console.WriteLine("do not have a module named " + module_name);
			}
		}

		private Hashtable modules;
	}
}


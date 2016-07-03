﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using MsgPack;
using MsgPack.Serialization;

namespace service
{
	public class channel : Ichannel
	{
		public delegate void DisconnectHandle(channel ch);
		public event DisconnectHandle onDisconnect;

		public channel(Socket _s)
		{
			s = _s;

			que = new Queue();

			recvbuf = new byte[1024 * 1024];
			tmpbuf = null;
			tmpbuflenght = 0;
			tmpbufoffset = 0;

			s.BeginReceive(recvbuf, 0, 1024*1024, 0, new AsyncCallback(this.onRead), this);
		}

		private void onRead(IAsyncResult ar)
		{
			channel ch = ar.AsyncState as channel;

			int read = ch.s.EndReceive(ar);

			if (read > 0)
			{
				if (tmpbufoffset == 0)
				{
					int offset = 0;
					do
					{
						Int32 len = BitConverter.ToInt32(recvbuf, offset);

						if (len < (read - 4))
						{
							read -= len + 4;
							offset += 4;

							MemoryStream _tmp = new MemoryStream();

							_tmp.Write(recvbuf, offset, len);
							offset += len;

							_tmp.Position = 0;

							var serializer = SerializationContext.Default.GetSerializer<ArrayList>();
							ArrayList unpackedObject = serializer.Unpack(_tmp);

							lock (que)
							{
								que.Enqueue(unpackedObject);
							}
						}
						else
						{
							if (tmpbuflenght == 0)
							{
								tmpbuflenght = 1024 * 1024 * 2;
								tmpbuf = new byte[tmpbuflenght];
							}

							while ((tmpbuflenght - tmpbufoffset) < read)
							{
								byte[] newtmpbuf = new byte[2 * tmpbuflenght];
								tmpbuf.CopyTo(newtmpbuf, 0);
								tmpbuf = newtmpbuf;
							}

							MemoryStream _tmp = new MemoryStream();
							_tmp.Write(recvbuf, offset, read);

							_tmp.ToArray().CopyTo(tmpbuf, tmpbufoffset);
							tmpbufoffset = read;

							break;
						}

					} while (true);
				}
				else if (tmpbufoffset > 0)
				{
					while ((tmpbuflenght - tmpbufoffset) < read)
					{
						byte[] newtmpbuf = new byte[2 * tmpbuflenght];
						tmpbuf.CopyTo(newtmpbuf, 0);
						tmpbuf = newtmpbuf;
					}

					MemoryStream _tmp_ = new MemoryStream();
					_tmp_.Write(recvbuf, 0, read);

					_tmp_.ToArray().CopyTo(tmpbuf, tmpbufoffset);
					tmpbufoffset += (Int32)_tmp_.Length;

					int offset = 0;
					do
					{
						Int32 len = BitConverter.ToInt32(tmpbuf, offset);

						if (len < (tmpbufoffset - 4))
						{
							tmpbufoffset -= len + 4;
							offset += 4;

							MemoryStream _tmp = new MemoryStream();

							_tmp.Write(tmpbuf, offset, len);
							offset += len;

							_tmp.Position = 0;

							var serializer = SerializationContext.Default.GetSerializer<ArrayList>();
							ArrayList unpackedObject = serializer.Unpack(_tmp);

							lock (que)
							{
								que.Enqueue(unpackedObject);
							}
						}
						else
						{
							MemoryStream _tmp = new MemoryStream();
							_tmp.Write(tmpbuf, offset, tmpbufoffset);

							_tmp.ToArray().CopyTo(tmpbuf, 0);

							break;
						}

					} while (true);
				}
			}
			else
			{
				onDisconnect(this);
			}
		}

		public ArrayList pop()
		{
			ArrayList _array = null;

			lock(que)
			{
				if (que.Count > 0)
				{
					_array = (ArrayList)que.Dequeue();
				}
			}

			return _array;
		}

		public void push(ArrayList _data)
		{
			var serializer = SerializationContext.Default.GetSerializer<ArrayList>();

			MemoryStream _tmp = new MemoryStream();
			serializer.Pack(_tmp, _data);

			s.Send(_tmp.ToArray());
		}

		private Socket s;
		private byte[] recvbuf;
		private byte[] tmpbuf;
		private Int32 tmpbuflenght;
		private Int32 tmpbufoffset;

		private Queue que;
	}
}


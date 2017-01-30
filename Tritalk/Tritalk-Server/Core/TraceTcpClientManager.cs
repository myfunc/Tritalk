using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tritalk.Core
{
    public class ActiveClient
    {
        public TcpClient Client { get; set; }
        public Task ActiveTask { get; set; }
    }

    class TraceTcpClientManager : ICollection<ActiveClient>
    {
        private List<ActiveClient> clients;

        public void Remove(TcpClient client)
        {
            var find = clients.Where(p => p.Client == client);
            find.ToList().ForEach(p => Remove(p));
        }

        public void Remove(Task client_task)
        {
            var find = clients.Where(p => p.ActiveTask == client_task);
            find.ToList().ForEach(p => Remove(p));
        }

        public int Count
        {
            get
            {
                return clients.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return (clients as ICollection<TcpClient>).IsReadOnly;
            }
        }

        public TraceTcpClientManager()
        {
            InitFields();
        }

        private void InitFields()
        {
            clients = new List<ActiveClient>();
        }

        public IEnumerator<ActiveClient> GetEnumerator()
        {
            return clients.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return clients.GetEnumerator();
        }

        public void Add(ActiveClient item)
        {
            clients.Add(item);
        }

        public void Clear()
        {
            clients.Clear();
        }

        public bool Contains(ActiveClient item)
        {
            return clients.Contains(item);
        }

        public void CopyTo(ActiveClient[] array, int arrayIndex)
        {
            clients.CopyTo(array, arrayIndex);
        }

        public bool Remove(ActiveClient item)
        {
            item.ActiveTask.Dispose(); // TODO: Небезопасная остановка задачи.
            item.Client.Close();
            return clients.Remove(item);
        }
    }
}

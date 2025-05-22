using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace VSW.Core.Services.Tracing.ElasticApm
{
    public class ElasticApmTracer : ITracer
    {
        public ITransaction CurrentTransaction
        {
            get
            {
                if (!_settings.Enabled)
                {
                    return null;
                }
                var innerTrans = Elastic.Apm.Agent.Tracer.CurrentTransaction;
                if (innerTrans == null)
                {
                    return null;
                }
                return new ElasticApmTransaction(innerTrans, _logger);
            }
        }

        public ISpan CurrentSpan
        {
            get
            {
                if (!_settings.Enabled)
                {
                    return null;
                }
                var innerSpan = Elastic.Apm.Agent.Tracer.CurrentSpan;
                if (innerSpan == null)
                {
                    return null;
                }
                var r = new ElasticApmSpan(CurrentTransaction, innerSpan);
                return r;
            }
        }

        private readonly TracingSettings _settings;
        private readonly ILogger<ElasticApmTracer> _logger;

        private bool Enabled { get; set; }

        public ElasticApmTracer(IOptions<TracingSettings> settings, ILogger<ElasticApmTracer> logger)
        {
            _settings = settings.Value;
            _logger = _settings.Log ? logger : null;
            Enabled = _settings != null && _settings.Enabled;
        }

        public object DeserializeTracingData(string tracingData)
        {
            if (!Enabled)
            {
                return null;
            }
            var result = Elastic.Apm.Api.DistributedTracingData.TryDeserializeFromString(tracingData);
            return result;
        }

        public ISpan StartChildSpan(string name, string type, string subType = null, string action = null)
        {
            if (!Enabled)
            {
                return null;
            }
            var currentSpan = CurrentSpan;
            if (currentSpan == null)
            {
                return StartSpan(name, type, subType, action);
            }

            var r = currentSpan.StartChildSpan(name, type, subType, action);
            return r;
        }

        public ISpan StartSpan(string name, string type, string subType = null, string action = null)
        {
            if (!Enabled)
            {
                return null;
            }

            if(CurrentTransaction == null)
            {
                StartTransaction(name, type);
            }

            var currentTrans = CurrentTransaction;
            var r = currentTrans.StartSpan(name, type, subType, action);
            return r;
        }

        public ITransaction StartTransaction(string name, string type, object tracingData = null)
        {
            if (!Enabled)
            {
                return null;
            }
            var data = tracingData as Elastic.Apm.Api.DistributedTracingData;
            var trans = Elastic.Apm.Agent.Tracer.StartTransaction(name, type, data);
            return new ElasticApmTransaction(trans, _logger);
        }
    }
}

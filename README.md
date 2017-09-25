# fake-elasticache-.NET-version
A .Net service simulating AWS's ElastiCache, backed by memcached, (for local development)

Inspired heavily by https://github.com/stevenjack/fake_elasticache and
                    https://github.com/dazoakley/fake-elasticache
                    
                    
You will now be able to point your ElastiCache endpoint to http://localhost:11212

 Usage Instuction for Nuget Package - NHibernate.Caches.Elasticache
 Amazon Elastic Cache checks for the string ".cfg" in your provided hostname.
 
 So edit Windows hosts.txt to add the following line
 127.0.0.1  amazon.cfg.localhost
 
and then add hostname in configuaration as amazon.cfg.localhost , port=11212

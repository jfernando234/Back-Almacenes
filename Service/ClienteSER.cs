using AutoMapper;
using Data;
using DTO;
using Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Service
{
    public class ClienteSER
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string cnBD = "";

        public ClienteSER(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            cnBD = _configuration.GetConnectionString("cn_bd_sige");
        }

        public List<DTO.ClienteListarDTO> listarAll()
        {
            try
            {
                var clienteDA = new ClienteDA(cnBD);
                var listaEntity = clienteDA.listarAll();
                var listaDto = _mapper.Map<List<DTO.ClienteListarDTO>>(listaEntity);
                return listaDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar todos los clientes: {ex.Message}", ex);
            }
        }

        public ClienteBE obtener(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El ID del cliente debe ser mayor a 0");

                var clienteDA = new ClienteDA(cnBD);
                var cliente = clienteDA.listar(id);
                
                if (cliente == null)
                    throw new Exception("Cliente no encontrado");

                return cliente;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener cliente: {ex.Message}", ex);
            }
        }

        public int agregar(ClienteAgregarDTO clienteDTO)
        {
            try
            {
                var clienteDA = new ClienteDA(cnBD);
                if (clienteDA.existeDocumento(clienteDTO.idTipoDocumento, clienteDTO.numeroDocumento))
                {
                    throw new Exception("Ya existe un cliente con este tipo y número de documento");
                }
                var clienteBE = _mapper.Map<ClienteBE>(clienteDTO);
                
                return clienteDA.agregar(clienteBE);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar cliente: {ex.Message}", ex);
            }
        }

        public int modificar(ClienteModificarDTO clienteDTO)
        {
            try
            {
                if (clienteDTO.idCliente <= 0)
                    throw new ArgumentException("El ID del cliente debe ser mayor a 0");

                var clienteDA = new ClienteDA(cnBD);
                
                var clienteExistente = clienteDA.listar(clienteDTO.idCliente);
                if (clienteExistente == null)
                {
                    throw new Exception("Cliente no encontrado");
                }

                if (clienteDA.existeDocumento(clienteDTO.idTipoDocumento, clienteDTO.numeroDocumento, clienteDTO.idCliente))
                {
                    throw new Exception("Ya existe otro cliente con este tipo y número de documento");
                }

                var clienteBE = _mapper.Map<ClienteBE>(clienteDTO);
                
                return clienteDA.modificar(clienteBE);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al modificar cliente: {ex.Message}", ex);
            }
        }

        public int deshabilitar(int id, string pcIp, string pcHost, int idUsuarioLogin)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El ID del cliente debe ser mayor a 0");

                var clienteDA = new ClienteDA(cnBD);
                
                if (!clienteDA.existeClientePorId(id))
                {
                    throw new Exception("Cliente no encontrado");
                }

                var clienteBE = new ClienteBE
                {
                    idCliente = id,
                    idUsuarioLogin = idUsuarioLogin,
                    pcIp = pcIp,
                    pcHost = pcHost
                };
                
                return clienteDA.deshabilitar(clienteBE);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al deshabilitar cliente: {ex.Message}", ex);
            }
        }

        public List<DTO.ClienteListarDTO> filtro(DateTime? inicio, DateTime? fin, string nombre)
        {
            try
            {
                var clienteDA = new ClienteDA(cnBD);
                var listaEntity = clienteDA.filtro(inicio, fin, nombre);
                var listaDto = _mapper.Map<List<DTO.ClienteListarDTO>>(listaEntity);
                return listaDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al aplicar filtro de clientes: {ex.Message}", ex);
            }
        }
    }
}
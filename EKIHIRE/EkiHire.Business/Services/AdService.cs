using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Configuration;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Exceptions;
using EkiHire.Core.Messaging.Email;
using EkiHire.Core.Messaging.Sms;
using EkiHire.Core.Model;
using EkiHire.Core.Utils;
using EkiHire.Data.Repository;
using EkiHire.Data.UnitOfWork;
using IPagedList;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using EkiHire.Core.Domain.Entities.Enums;
namespace EkiHire.Business.Services
{
    public interface IAdService
    {
        Task<bool> AddAd(AdDTO model);
        Task<bool> CloseAd(long model);
        Task<bool> EditAd(long model);
        Task<bool> PromoteAd(long model);
        Task<bool> CreateAdItem(long[] AdIds);
        Task<bool> AddAdToItem(long[] AdIds, long ItemId);
        Task<bool> GroupAdItem(long[] ItemIds, string groupname);
        Task<bool> AddToCart(long Id);
        Task<bool> RemoveFromCart(long Id);
    }
    public class AdService: IAdService
    {
        private readonly IRepository<Ad> repository;
        public AdService()
        {

        }
        public async Task<bool> AddAd(AdDTO model)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public async Task<bool> CloseAd(long model)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> EditAd(long model)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public async Task<bool> PromoteAd(long model)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> CreateAdItem(long[] AdIds)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> AddAdToItem(long[] AdIds, long ItemId)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> GroupAdItem(long[] ItemIds, string groupname)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> AddToCart(long Id)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> RemoveFromCart(long Id)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}

// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetObjectModel.
 * 
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This library is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
 * License for more details. 
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>,
 * or write to the Free Software Foundation, Inc., 51 Franklin Street,
 * Fifth Floor, Boston, MA 02110-1301, USA.
 */

using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace XSpect.MetaTweet.Objects
{
    public sealed class StorageObjectIdConverter
        : JsonConverter,
          IDataContractSurrogate
    {
        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, ((IStorageObjectId) value).HexString);
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            String value = (String) serializer.Deserialize(reader, typeof(String));
            switch (value.Length)
            {
                case AccountId.HexStringLength:
                    return new AccountId(value);
                case ActivityId.HexStringLength:
                    return new ActivityId(value);
                case AdvertisementId.HexStringLength:
                    return new AdvertisementId(value);
                default:
                    throw new InvalidOperationException();
            }
        }

        public override Boolean CanConvert(Type objectType)
        {
            return typeof(IStorageObjectId).IsAssignableFrom(objectType);
        }

        public Type GetDataContractType(Type type)
        {
            return this.CanConvert(type)
                ? typeof(String)
                : type;
        }

        public Object GetObjectToSerialize(Object obj, Type targetType)
        {
            return obj is IStorageObjectId
                ? ((IStorageObjectId) obj).HexString
                : obj;
        }

        public Object GetDeserializedObject(Object obj, Type targetType)
        {
            String str = (String) obj;
            return this.CanConvert(targetType)
                ? targetType == typeof(AccountId)
                      ? new AccountId(str)
                      : targetType == typeof(ActivityId)
                            ? (Object) new ActivityId(str)
                            : new AdvertisementId(str)
                : obj;
        }

        public Object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public Object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }

        public Type GetReferencedTypeOnImport(String typeName, String typeNamespace, Object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }
    }
}
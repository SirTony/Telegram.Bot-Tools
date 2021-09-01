using System;
using System.Collections.Immutable;

namespace Telegram.Bot.CommandHandler
{
    /// <summary>
    ///     Alternative commands to invoke the same method.
    /// </summary>
    [AttributeUsage( AttributeTargets.Method )]
    public class AliasesAttribute : Attribute
    {
        /// <summary>
        ///     Alternative commands to invoke the same method.
        /// </summary>
        public ImmutableArray<string> Aliases { get; }

        /// <summary>
        ///     Alternative commands to invoke the same method.
        /// </summary>
        /// <param name="aliases">Aliases of the command</param>
        public AliasesAttribute( params string[] aliases )
        {
            if( aliases is null ) throw new ArgumentNullException( nameof( aliases ) );

            var builder = ImmutableArray.CreateBuilder<string>();

            foreach( var alias in aliases )
            {
                if( String.IsNullOrWhiteSpace( alias ) )
                    throw new ArgumentNullException( nameof( alias ), "alias cannot be null or empty" );

                builder.Add( alias );
            }

            this.Aliases = builder.ToImmutable();
        }
    }
}